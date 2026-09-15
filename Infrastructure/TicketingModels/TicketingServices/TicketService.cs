using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.IServices.V2;
using Services.IServices.V2.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;
using static Common.Utilities.GlobalEnums;
using TicketStatus = Common.Utilities.GlobalEnums.TicketStatus;

namespace Services.Services.V2
{
    public class TicketService : CrudService<TicketCuDto, TicketListDto, TicketSearchDto, Ticket, long>, ITicketService
    {
        private readonly ITicketUserResponseService _TicketUserResponseService;
        private readonly ITicketAttachmentService _TicketAttachmentService;
        private readonly ITicketStatusHistoryService _TicketStatusHistoryService;
        private readonly INotificationService _NotificationService;
        private readonly IRepository<TicketStatusHistory> _TicketStatusHistoryRepository;
        private readonly IRepository<TicketReferHistory> _TicketReferHistoryRepository;
        private readonly IRepository<TicketUserResponse> _TicketUserResponseRepository;
        private readonly IUsersService _usersService;
        private readonly IRepository<TicketCategory> _categoriesRepository;
        private readonly IMapper _Mapper;
        private readonly IRepository<Ticket> _repository;
        //private readonly ITicketDashboardStatService _TicketDashboardStatService;
        public TicketService(IRepository<Ticket> repository,
            ITicketUserResponseService ticketUserResponseService,
            ITicketAttachmentService ticketAttachmentService,
            ITicketStatusHistoryService ticketStatusHistoryService,
            INotificationService notificationService,
            IRepository<TicketStatusHistory> ticketStatusHistoryRepository,
            IRepository<TicketReferHistory> ticketReferHistoryRepository,
            IRepository<TicketUserResponse> ticketUserResponseRepository,
            IUsersService usersService,
            IRepository<TicketCategory> CategoriesRepository,
            //ITicketDashboardStatService ticketDashboardStatService
            IMapper mapper) : base(repository, mapper)
        {
            _TicketUserResponseService = ticketUserResponseService;
            _TicketAttachmentService = ticketAttachmentService;
            _TicketStatusHistoryService = ticketStatusHistoryService;
            _NotificationService = notificationService;
            _TicketReferHistoryRepository = ticketReferHistoryRepository;
            _TicketStatusHistoryRepository = ticketStatusHistoryRepository;
            _TicketUserResponseRepository = ticketUserResponseRepository;
            _usersService = usersService;
            _categoriesRepository = CategoriesRepository;
            _Mapper = mapper;
            _repository = repository;
            //_TicketDashboardStatService = ticketDashboardStatService;
        }

        public async Task<ApiResult<TicketListDto>> CreateTicket(TicketCuDto model, List<IFormFile> files, long CreatedBy, string folderPath, CancellationToken cancellationToken)
        {
            try
            {
                var newTicket = new ApiResult<TicketListDto>(true, ApiResultStatusCode.Success, null);
                if (model.Id > 0)
                {
                    return new ApiResult<TicketListDto>(false, ApiResultStatusCode.TicketIdExists, null, null);
                }
                if (model.Description is null)
                {
                    return new ApiResult<TicketListDto>(false, ApiResultStatusCode.TicketDescriptionEmpty, null, null);
                }

                model.StatusId = (int)GlobalEnums.TicketStatus.Open;

                newTicket = await base.Create(model, CreatedBy, cancellationToken);

                var ticketStatusHistory = new TicketStatusHistory
                {
                    TicketId = newTicket.Data.Id,
                    StatusId = (int)GlobalEnums.TicketStatus.Open,
                };

                await _TicketStatusHistoryRepository.AddAsync(ticketStatusHistory, cancellationToken);

                var ticketUserResponseCuDto = new TicketUserResponseCuDto
                {
                    TicketId = newTicket.Data.Id,
                    Description = model.Description,
                    UserId = CreatedBy
                };

                var newTicketUserResponse = await CreateTicketResponse(ticketUserResponseCuDto, files, CreatedBy, folderPath, cancellationToken);

                var result = await _repository.TableNoTracking.ProjectTo<TicketListDto>(_Mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == newTicket.Data.Id);

                return result;
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                return new ApiResult<TicketListDto>(false, ApiResultStatusCode.ServerError, null, ApiResultStatusCode.ServerError.ToDisplay());

            }
        }
        public async Task<ApiResult<TicketListDto>> CreateTicketResponse(TicketUserResponseCuDto model, List<IFormFile> files, long CreatedBy, string folderPath, CancellationToken cancellationToken)
        {
            try
            {
                var referedToUserId = _TicketReferHistoryRepository.TableNoTracking.Where(x => x.TicketId == model.TicketId).OrderByDescending(src => src.Id).FirstOrDefault();
                var TicketCreator = Repository.TableNoTracking.Where(t => t.Id == model.TicketId).FirstOrDefault();

                var lastTicketResponse = await _TicketUserResponseRepository.TableNoTracking
                    .Where(p => p.TicketId.Equals(model.TicketId))
                    .OrderByDescending(x => x.Id)
                    .ProjectTo<TicketUserResponseListDto>(Mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                var user = await _usersService.GetUserById(CreatedBy, cancellationToken);

                if (TicketCreator.CreatedBy == CreatedBy || referedToUserId.ToUserId == CreatedBy)
                {
                    var entity = model.ToEntity(_Mapper);

                    await _TicketUserResponseRepository.AddAsync(entity, cancellationToken);

                    var newTicketUserResponse =await _TicketUserResponseRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == entity.Id);
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i].Length == 0)
                            throw new BadRequestException("حجم فایل تصویر صفر است.");

                        var filePath = FileOperationsExtension.GetUniqueFilePath(folderPath, "T" + Path.GetExtension(files[i].FileName));

                        if (String.IsNullOrEmpty(filePath))
                            throw new BadRequestException("مسیر ذخیره فایل درست نیست یا نام آن اشکال دارد.");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await files[i].CopyToAsync(stream);
                        }

                        var ticketAttachmentCuDto = new TicketAttachmentCuDto
                        {
                            TicketResponseId = newTicketUserResponse.Id,
                            FileName = Path.GetFileName(filePath),
                            FileExt = Path.GetExtension(filePath),
                            SizeInBytes = files[i].Length
                        };
                        var newTicketAttachment = await _TicketAttachmentService.Create(ticketAttachmentCuDto, CreatedBy, cancellationToken);
                    }
                    if (lastTicketResponse is not null)
                    { 
                        if (lastTicketResponse.CreatedBy != CreatedBy)
                        {
                            var identityUser = await _usersService.GetUserById(lastTicketResponse.CreatedBy , cancellationToken);
                            var notificationCuDto = new NotificationCuDto()
                            {
                                Title = "پاسخ تیکت",
                                Description = "تیکت به شماره " + model.TicketId + " توسط " + user.FullName + " پاسخ داده شد",
                                PersonId = identityUser.PersonId,
                                FrontendRouteId = 2,
                               // ContextId = model.TicketId,
                                SendSMS = false,
                                SendInApp = true,
                                SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                                Seen = false,
                            };
                            await _NotificationService.Create(notificationCuDto, CreatedBy, cancellationToken);
                        }
                    }

                    var result = await _repository.TableNoTracking.ProjectTo<TicketListDto>(_Mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(x => x.Id == model.TicketId);

                    return result;
                }
                else
                {
                    return new ApiResult<TicketListDto>(false, ApiResultStatusCode.TicketIsNotRefered, null);
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<TicketListDto>(false, ApiResultStatusCode.ServerError, null, ApiResultStatusCode.ServerError.ToDisplay());
            }
        }
        public override async Task<ApiResult<IPagedList<TicketListDto>>> Get(TicketSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            expression = expression.AndExpression(src => src.TicketStatusId == (int)GlobalEnums.TicketStatus.Open);
            expression = expression.AndExpression(src => src.ReferedUserId == null);

            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                //.ThenBy(src => src.TicketStatusId)
                //.ThenByDescending(src => src.LastModified)
                .Where(expression).ProjectTo<TicketListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);
            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketListDto>>(true, ApiResultStatusCode.Success,
                result,
                null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }
        public async Task<ApiResult<IPagedList<TicketBriefListDto>>> GetBrief(TicketSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            expression = expression.AndExpression(src => src.TicketStatusId == (int)GlobalEnums.TicketStatus.Open);
            expression = expression.AndExpression(src => src.ReferedUserId == null);

            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                //.ThenBy(src => src.TicketStatusId)
                //.ThenByDescending(src => src.LastModified)
                .Where(expression).ProjectTo<TicketBriefListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);
            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketBriefListDto>>(true, ApiResultStatusCode.Success,
                result,
                null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }
        public async Task<ApiResult<TicketReferListDto>> CreateTicketRefer(TicketReferCuDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            var existingRecord = await Repository.TableNoTracking.SingleOrDefaultAsync(p => p.Id.Equals(model.TicketId), cancellationToken);
            var hasSupeAdminRole = await _usersService.UserIsInRole(CreatedBy, "SuperAdmin", cancellationToken);

            if (!hasSupeAdminRole.IsSuccess is true)
            {
                if (existingRecord.TicketStatusId == 2 || (existingRecord.ReferedUserId is not null && existingRecord.ReferedUserId != CreatedBy))
                {
                    return new ApiResult<TicketReferListDto>(false, ApiResultStatusCode.BadRequest, null, "تیکت بسته شده است یا در اختیار شما نیست.");
                }
            }

            var ticketReferCuDto = new TicketReferCuDto
            {
                TicketId = model.TicketId,
                FromUserId = model.FromUserId,
                ToUserId = model.ToUserId,
            };
            var entity = ticketReferCuDto.ToEntity(_Mapper);
            await _TicketReferHistoryRepository.AddAsync(entity, cancellationToken);

            existingRecord.TicketStatusId = (int)GlobalEnums.TicketStatus.InProgress;
            existingRecord.ReferedUserId = model.ToUserId;
            await Repository.UpdateAsync(existingRecord, cancellationToken);

            var ticketStatusHistory = new TicketStatusHistory
            {
                TicketId = model.TicketId,
                StatusId = existingRecord.TicketStatusId,
            };
            await _TicketStatusHistoryRepository.AddAsync(ticketStatusHistory, cancellationToken);
            var user = await _usersService.GetUserById(model.ToUserId, cancellationToken);
            if (existingRecord.CreatedBy != model.ToUserId)
            {
                var identityUser = await _usersService.GetUserById((long)existingRecord.CreatedBy, cancellationToken);
                var notificationCuDto = new NotificationCuDto()
                {
                    Title = "تیکت",
                    Description = "تیکت شماره " + model.TicketId + " به " + ((model.FromUserId == model.ToUserId) ? "شما" : user.FullName) + " ارجاع شد",
                    PersonId = identityUser.PersonId,
                    FrontendRouteId = 2,
                   // ContextId = model.TicketId,
                    SendSMS = false,
                    SendInApp = true,
                    SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                    Seen = false,
                };
                await _NotificationService.Create(notificationCuDto, CreatedBy, cancellationToken);
            }

            if (CreatedBy != model.ToUserId)
            {
                var identityUser = await _usersService.GetUserById(model.ToUserId, cancellationToken);
                var referedNotificationCuDto = new NotificationCuDto()
                {
                    Title = "تیکت",
                    Description = "تیکت شماره " + model.TicketId + " به شما ارجاع شد",
                    PersonId = identityUser.PersonId,
                    FrontendRouteId = 2,
                    ContextId= (int?)model.TicketId,
                    SendSMS = false,
                    SendInApp = true,
                    SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                    Seen = false,
                };
                await _NotificationService.Create(referedNotificationCuDto, CreatedBy, cancellationToken);
            }
            return new ApiResult<TicketReferListDto>(true, ApiResultStatusCode.Success, null, null);
        }
        public async Task<ApiResult<IPagedList<TicketListDto>>> GetTicketList(TicketSearchDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            var hasAllTicketsRole = await _usersService.UserIsInRole(CreatedBy, "Ticket-AllTickets", cancellationToken);
            var hasSuperAdminRole = await _usersService.UserIsInRole(CreatedBy, "SuperAdmin", cancellationToken);

            if (hasAllTicketsRole.IsSuccess is true || hasSuperAdminRole.IsSuccess is true)
            {
                return await Get(model, cancellationToken);
            }

            //var ticketingCategories = await _categoriesRepository.TableNoTracking.ToLookup(p => p.RoleId).Select(p => p.First()).ToListAsync(cancellationToken);

            //var userTicketingRoles = new LinkedList<long>();
            //foreach (var category in ticketingCategories)
            //{
            //    var isInRole = await _usersService.UserIsInRoleByRoleId(CreatedBy, category.FRoleId, cancellationToken);
            //    if (isInRole.Data.Result)
            //        userTicketingRoles.AddLast(category.FRoleId);
            //}
            //long[] userTicketingRolesIds = new long[userTicketingRoles.Count];
            //userTicketingRoles.CopyTo(userTicketingRolesIds, 0);

            //if (userTicketingRoles.Count() == 0)
            //    return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.NotFound, null, ApiResultStatusCode.NotFound.ToDisplay());

            //model.DesiredRoleIds = userTicketingRolesIds;
            var expression = model.GenerateExpression(model);
            expression = expression.AndExpression(src => src.TicketStatusId == (int)GlobalEnums.TicketStatus.Open);
            expression = expression.AndExpression(src => src.ReferedUserId == null);

            if (model.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                //.ThenBy(src => src.TicketStatusId)
                .ThenByDescending(src => src.LastModified)
                .Where(expression)
                .ProjectTo<TicketListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }
        public async Task<ApiResult<IPagedList<TicketBriefListDto>>> GetBriefTicketList(TicketSearchDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            var hasAllTicketsRole = await _usersService.UserIsInRole(CreatedBy, "Ticket-AllTickets", cancellationToken);
            var hasSuperAdminRole = await _usersService.UserIsInRole(CreatedBy, "SuperAdmin", cancellationToken);

            if (hasAllTicketsRole.IsSuccess is true  || hasSuperAdminRole.IsSuccess is true)
            {
                return await GetBrief(model, cancellationToken);
            }

            //var ticketingCategories = await _categoriesRepository.TableNoTracking.ToLookup(p => p.FRoleId).Select(p => p.First()).ToListAsync(cancellationToken);

            //var userTicketingRoles = new LinkedList<long>();
            //foreach (var category in ticketingCategories)
            //{
            //    var isInRole = await _usersService.UserIsInRoleByRoleId(CreatedBy, category.FRoleId, cancellationToken);
            //    if (isInRole.Data.Result)
            //        userTicketingRoles.AddLast(category.FRoleId);
            //}
            //long[] userTicketingRolesIds = new long[userTicketingRoles.Count];
            //userTicketingRoles.CopyTo(userTicketingRolesIds, 0);

            //if (userTicketingRoles.Count() == 0)
            //    return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null, ApiResultStatusCode.NotFound.ToDisplay());

            //model.DesiredRoleIds = userTicketingRolesIds;
            var expression = model.GenerateExpression(model);
            expression = expression.AndExpression(src => src.TicketStatusId == (int)GlobalEnums.TicketStatus.Open);
            expression = expression.AndExpression(src => src.ReferedUserId == null);

            if (model.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                //.ThenBy(src => src.TicketStatusId)
                .ThenByDescending(src => src.LastModified)
                .Where(expression)
                .ProjectTo<TicketBriefListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketBriefListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }
        public async Task<ApiResult<IPagedList<TicketBriefListDto>>> TicketArchivedList(TicketSearchDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            var hasSuperAdminRole = await _usersService.UserIsInRole(CreatedBy, "SuperAdmin", cancellationToken);

            if (hasSuperAdminRole.IsSuccess is true)
            {
                var _expression = model.GenerateExpression(model);
                if (model.RecordsPerPage > 50)
                    return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                        null);

                var _result = await Repository.TableNoTracking
                    .OrderByDescending(src => src.IsActive)
                    .Where(_expression)
                    .ProjectTo<TicketBriefListDto>(Mapper.ConfigurationProvider)
                    .ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);
                if (_result.Count == 0)
                    return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);

                return new ApiResult<IPagedList<TicketBriefListDto>>(true, ApiResultStatusCode.Success,
                    _result,
                    null,
                    _result.TotalItemCount,
                    _result.PageNumber, _result.PageCount);
            }

            //var ticketingCategories = await _categoriesRepository.TableNoTracking.ToLookup(p => p.FRoleId).Select(p => p.First()).ToListAsync(cancellationToken);

            //var userTicketingRoles = new LinkedList<long>();
            //foreach (var category in ticketingCategories)
            //{
            //    var isInRole = await _usersService.UserIsInRoleByRoleId(CreatedBy, category.FRoleId, cancellationToken);
            //    if (isInRole.Data.Result)
            //        userTicketingRoles.AddLast(category.FRoleId);
            //}
            //long[] userTicketingRolesIds = new long[userTicketingRoles.Count];
            //userTicketingRoles.CopyTo(userTicketingRolesIds, 0);

            //if (userTicketingRoles.Count() == 0)
            //    return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null, ApiResultStatusCode.NotFound.ToDisplay());

            //model.DesiredRoleIds = userTicketingRolesIds;
            var expression = model.GenerateExpression(model);

            if (model.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                .ThenByDescending(src => src.LastModified)
                .Where(expression)
                .ProjectTo<TicketBriefListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketBriefListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }
        public async Task<ApiResult<TicketStatusHistoryListDto>> ChangeTicketStatus(TicketStatusHistoryCuDto searchDto, long CreatedBy, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUserById(CreatedBy, cancellationToken);

            var TicketStatusHistoryCuDto = new TicketStatusHistoryCuDto
            {
                TicketId = searchDto.TicketId,
                StatusId = searchDto.StatusId,
            };

            // await base.Create(TicketStatusHistoryCuDto, 2, cancellationToken);
            //var StatusHistory = await _TicketStatusHistoryService.Create(ticketStatusHistoryCuDto, CreatedBy, cancellationToken);

            var entity = searchDto.ToEntity(_Mapper);
            await _TicketStatusHistoryRepository.AddAsync(entity, cancellationToken);

            var existingRecord = await Repository.TableNoTracking.SingleOrDefaultAsync(p => p.Id.Equals(searchDto.TicketId), cancellationToken);
            existingRecord.TicketStatusId = searchDto.StatusId;

            await Repository.UpdateAsync(existingRecord, cancellationToken);

            if (searchDto.StatusId == (int)GlobalEnums.TicketStatus.Close)
            {
                if (existingRecord.CreatedBy != CreatedBy)
                {
                    var identityUser = await _usersService.GetUserById((long)existingRecord.CreatedBy, cancellationToken);
                    var notificationCuDto = new NotificationCuDto()
                    {
                        Title = "تغییر وضعیت تیکت",
                        Description = "تیکت شماره " + searchDto.TicketId + " در تاریخ " + PersianDateExtensions.ToPersianDate(DateTime.Today) + " توسط " + (CreatedBy == 2 ? "سیستم" : user.FullName) + " بسته شد",
                        PersonId = identityUser.PersonId,
                        FrontendRouteId = 2,
                        //ContextId = searchDto.TicketId,
                        SendSMS = false,
                        SendInApp = true,
                        SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                        Seen = false,
                    };
                    await _NotificationService.Create(notificationCuDto, CreatedBy, cancellationToken);
                }
            }

            return new ApiResult<TicketStatusHistoryListDto>(true, ApiResultStatusCode.Success, null, null);
        }
        public async Task<ApiResult<IPagedList<TicketListDto>>> GetAll(TicketSearchDto model, long userId, CancellationToken cancellationToken)
        {
            var hasTicketReport = await _usersService.UserIsInRole(userId, "Ticket-Report", cancellationToken);
            var hasSuperAdmin = await _usersService.UserIsInRole(userId, "SuperAdmin", cancellationToken);
            if (hasSuperAdmin.IsSuccess is true || hasTicketReport.IsSuccess is true)
            {
                var listResult = new List<TicketListDto>();

                var expression = model.GenerateExpression(model);
                var result = await Repository.TableNoTracking
                    .OrderByDescending(src => src.IsActive)
                    .ThenByDescending(src => src.LastModified)
                    .Where(expression)
                    .ProjectTo<TicketListDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();

                if (result.Count == 0)
                    return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.NotFound, null);

                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Open));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.InProgress));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Close));

                var PagedListResult = await listResult.ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

                return new ApiResult<IPagedList<TicketListDto>>(true, ApiResultStatusCode.Success,
                    PagedListResult,
                    null,
                    PagedListResult.TotalItemCount,
                    PagedListResult.PageNumber, PagedListResult.PageCount);
            }
            else if ((model.CreatedBy is not null && model.CreatedBy != 0) || (model.ReferredUserId is not null && model.ReferredUserId != 0))
            {
                var listResult = new List<TicketListDto>();

                var expression = model.GenerateExpression(model);
                var result = await Repository.TableNoTracking
                    .OrderByDescending(src => src.IsActive)
                    .ThenByDescending(src => src.LastModified)
                    .Where(expression)
                    .ProjectTo<TicketListDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();

                if (result.Count == 0)
                    return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.NotFound, null);

                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Open));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.InProgress));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Close));

                var PagedListResult = await listResult.ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

                return new ApiResult<IPagedList<TicketListDto>>(true, ApiResultStatusCode.Success,
                    PagedListResult,
                    null,
                    PagedListResult.TotalItemCount,
                    PagedListResult.PageNumber, PagedListResult.PageCount);
            }
            return new ApiResult<IPagedList<TicketListDto>>(false, ApiResultStatusCode.NotFound, null);
        }
        public async Task<ApiResult<IPagedList<TicketBriefListDto>>> GetAllBrief(TicketSearchDto model, long userId, CancellationToken cancellationToken)
        {
            var hasTicketReport = await _usersService.UserIsInRole(userId, "TicketReport", cancellationToken);
            var hasSuperAdmin = await _usersService.UserIsInRole(userId, "SuperAdmin", cancellationToken);
            if (hasSuperAdmin.IsSuccess is true || hasTicketReport.IsSuccess is true)
            {
                var listResult = new List<TicketBriefListDto>();

                var expression = model.GenerateExpression(model);
                var result = await Repository.TableNoTracking
                    .OrderByDescending(src => src.IsActive)
                    .ThenByDescending(src => src.LastModified)
                    .Where(expression)
                    .ProjectTo<TicketBriefListDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();

                if (result.Count == 0)
                    return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);

                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Open));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.InProgress));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Close));

                var PagedListResult = await listResult.ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

                return new ApiResult<IPagedList<TicketBriefListDto>>(true, ApiResultStatusCode.Success,
                    PagedListResult,
                    null,
                    PagedListResult.TotalItemCount,
                    PagedListResult.PageNumber, PagedListResult.PageCount);
            }
            else if ((model.CreatedBy is not null && model.CreatedBy != 0) || (model.ReferredUserId is not null && model.ReferredUserId != 0))
            {
                var listResult = new List<TicketBriefListDto>();

                var expression = model.GenerateExpression(model);
                var result = await Repository.TableNoTracking
                    .OrderByDescending(src => src.IsActive)
                    .ThenByDescending(src => src.LastModified)
                    .Where(expression)
                    .ProjectTo<TicketBriefListDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();

                if (result.Count == 0)
                    return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);

                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Open));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.InProgress));
                listResult.AddRange(result.Where(x => x.StatusId == (int)TicketStatus.Close));

                var PagedListResult = await listResult.ToPagedListAsync(model.PageNumber ?? 1, model.RecordsPerPage ?? 10, cancellationToken);

                return new ApiResult<IPagedList<TicketBriefListDto>>(true, ApiResultStatusCode.Success,
                    PagedListResult,
                    null,
                    PagedListResult.TotalItemCount,
                    PagedListResult.PageNumber, PagedListResult.PageCount);
            }
            return new ApiResult<IPagedList<TicketBriefListDto>>(false, ApiResultStatusCode.NotFound, null);
        }
        public async Task<ApiResult<TicketListDto>> Get(long id, long CreatedBy, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TicketListDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (dto == null)
                return new ApiResult<TicketListDto>(false, ApiResultStatusCode.NotFound, null);

            return dto;
        }
      
        public async Task<ApiResult<TicketReportDto>> TicketReport(TicketSearchDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            TimeSpan difference;
            TimeSpan sum = new TimeSpan();

            var expression = model.GenerateExpression(model);
            var tickets = Repository.TableNoTracking.Where(expression)
                .Include(x => x.TicketCategory)
                .Include(x => x.ReferedUser)
                .Include(x => x.ReferedUser.Person);

            var closeList = tickets.Where(p => p.TicketStatusId == (int)TicketStatus.Close).ToList();
            for (int i = 0; i < closeList.Count; i++)
            {
                difference = (DateTime)closeList[i].LastModified - closeList[i].Created;
                 sum += difference;
            }

            var inProgressList = tickets.Where(p => p.TicketStatusId == (int)TicketStatus.InProgress).ToList();
            for (int i = 0; i < inProgressList.Count; i++)
            {
                difference = (DateTime)inProgressList[i].LastModified - inProgressList[i].Created;
                sum += difference;
            }

            //MostUsedCategoriesDto mostUsedCategories = new MostUsedCategoriesDto;
            var ticketReportCategoryDto = (from t in tickets
                                    group t by new
                                    { t.TicketCategoryId, t.TicketCategory.Title } into Tic
                                    select new TicketReportCategoryDto
                                    {
                                        CategoryId = Tic.Key.TicketCategoryId,
                                        CategoryTitle = Tic.Key.Title,
                                        AllTicketsCount = Tic.Count(),
                                        OpenTicketsCount = Tic.Where(x => x.TicketStatusId == 1).Count(),
                                        InProgressTicketsCount = Tic.Where(x => x.TicketStatusId == 3).Count(),
                                        ClosedTicketsCount = Tic.Where(x => x.TicketStatusId == 2).Count(),
                                    }).OrderByDescending(x => x.AllTicketsCount).ToList();
            
            var ticketReportUserDto = (from t in tickets
                                       group t by new { t.ReferedUserId, t.ReferedUser.Person.FirstName} into Tic
                                    select new TicketReportUserDto
                                    {
                                        UserId = Tic.Key.ReferedUserId,
                                        FullName = Tic.Key.FirstName,
                                        InProgress = Tic.Where(x => x.TicketStatusId == 3).Count(),
                                        Closed = Tic.Where(x => x.TicketStatusId == 2).Count(),
                                    }).OrderByDescending(x => x.InProgress).ToList();

            ticketReportUserDto.RemoveAll(x => x.UserId == null);

            var TicketReportDto = new TicketReportDto
            {
                AllTicketsCount = tickets.Count(),
                OpenTicketsCount = tickets.Where(p => p.TicketStatusId == (int)TicketStatus.Open).Count(),
                InProgressTicketsCount = tickets.Where(p => p.TicketStatusId == (int)TicketStatus.InProgress).Count(),
                ClosedTicketsCount = tickets.Where(p => p.TicketStatusId == (int)TicketStatus.Close).Count(),
                TicketReportCategoryDto = ticketReportCategoryDto,
                TicketReportUserDto = ticketReportUserDto,
            };
            //TicketReportDto.MostUsedCategories.Add(mostUsedCategories);

            return new ApiResult<TicketReportDto>(true, ApiResultStatusCode.Success, TicketReportDto, null);
        }
        public async Task AutomaticTicketClosing(CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            var tickets = Repository.TableNoTracking.Where(c => c.TicketStatusId == (int)GlobalEnums.TicketStatus.InProgress);
            foreach (var ticket in tickets)
            {
                var ticketResponse = _TicketUserResponseRepository.TableNoTracking
                    .Where(p => p.TicketId == ticket.Id)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();

                if (ticketResponse.Result.CreatedBy != ticket.CreatedBy
                    && ((DateTime)ticketResponse.Result.LastModified).AddHours(72) < now)
                {
                    var ticketStatusHistoryCuDto = new TicketStatusHistoryCuDto
                    {
                        TicketId = ticket.Id,
                        StatusId = (int)GlobalEnums.TicketStatus.Close,
                    };

                    await ChangeTicketStatus(ticketStatusHistoryCuDto, 2, cancellationToken);
                }
            }
            return;
        }
        public async Task<ApiResult<TicketListDto>> ChangeTicketCategory(TicketCuDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            var existingTicket = await Repository.TableNoTracking.Where(src => src.Id == model.Id).FirstOrDefaultAsync();
            var existingCategory = await _categoriesRepository.TableNoTracking.Where(src => src.Id == model.CategoryId).FirstOrDefaultAsync();

            if (existingTicket is null || existingCategory is null)
                return new ApiResult<TicketListDto>(false, ApiResultStatusCode.NotFound, null);

            var identityUser = await _usersService.GetUserById((long)existingTicket.CreatedBy, cancellationToken);
            var referedNotificationCuDto = new NotificationCuDto()
            {
                Title = "تیکت",
                Description = "دسته بندی تیکت شماره " + model.Id + " از " + existingTicket.TicketCategory.Title + " به " + existingCategory.Title + " تغییر کرد.",
                PersonId = identityUser.PersonId,
                FrontendRouteId = 2,
               // ContextId = model.Id,
                SendSMS = false,
                SendInApp = true,
                SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                Seen = false,
            };
            await _NotificationService.Create(referedNotificationCuDto, CreatedBy, cancellationToken);

            existingTicket.TicketCategoryId = model.CategoryId;
            await Repository.UpdateAsync(existingTicket, cancellationToken);

            var result = await Repository.TableNoTracking
                .ProjectTo<TicketListDto>(_Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(src => src.Id == existingTicket.Id);

            if (result is null)
                return new ApiResult<TicketListDto>(false, ApiResultStatusCode.UpdateFailed, null);

            return new ApiResult<TicketListDto>(true, ApiResultStatusCode.Success, result, null);
        }
        public async Task<ApiResult<TicketReferListDto>> ReferClosedTicket(TicketReferCuDto model, long CreatedBy, CancellationToken cancellationToken)
        {
            var existingRecord = await Repository.TableNoTracking.SingleOrDefaultAsync(p => p.Id.Equals(model.TicketId), cancellationToken);
            var hasSupeAdminRole = await _usersService.UserIsInRole(CreatedBy, "SuperAdmin", cancellationToken);

            if (!hasSupeAdminRole.IsSuccess)
            {
                if (existingRecord.TicketStatusId == 2 || (existingRecord.ReferedUserId is not null && existingRecord.ReferedUserId != CreatedBy))
                {
                    return new ApiResult<TicketReferListDto>(false, ApiResultStatusCode.BadRequest, null, "تیکت بسته شده است یا در اختیار شما نیست.");
                }
            }

            var ticketReferCuDto = new TicketReferCuDto
            {
                TicketId = model.TicketId,
                FromUserId = model.FromUserId,
                ToUserId = model.ToUserId,
            };
            var entity = ticketReferCuDto.ToEntity(_Mapper);
            await _TicketReferHistoryRepository.AddAsync(entity, cancellationToken);

            existingRecord.TicketStatusId = (int)TicketStatus.InProgress;
            existingRecord.ReferedUserId = model.ToUserId;
            await Repository.UpdateAsync(existingRecord, cancellationToken);

            var ticketStatusHistory = new TicketStatusHistory
            {
                TicketId = model.TicketId,
                StatusId = existingRecord.TicketStatusId,
            };
            await _TicketStatusHistoryRepository.AddAsync(ticketStatusHistory, cancellationToken);
            var user = await _usersService.GetUserById(model.ToUserId, cancellationToken);

            if (existingRecord.CreatedBy != model.ToUserId)
            {
                var notificationCuDto = new NotificationCuDto()
                {
                    Title = "تیکت",
                    Description = "تیکت شماره " + model.TicketId + " به " + ((model.FromUserId == model.ToUserId) ? "شما" : user.FullName) + " ارجاع شد",
                    PersonId = existingRecord.CreatedBy,
                    FrontendRouteId = 2,
                    //ContextId = model.TicketId,
                    SendSMS = false,
                    SendInApp = true,
                    SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                    Seen = false,
                };
                await _NotificationService.Create(notificationCuDto, CreatedBy, cancellationToken);
            }

            if (CreatedBy != model.ToUserId)
            {
                var identityUser = await _usersService.GetUserById(model.ToUserId, cancellationToken);
                var referedNotificationCuDto = new NotificationCuDto()
                {
                    Title = "تیکت",
                    Description = "تیکت شماره " + model.TicketId + " به شما ارجاع شد",
                    PersonId = identityUser.PersonId,
                    FrontendRouteId = 2,
                    //ContextId = model.TicketId,
                    SendSMS = false,
                    SendInApp = true,
                    SendDate = PersianDateExtensions.ToPersianDate(DateTime.Today),
                    Seen = false,
                };
                await _NotificationService.Create(referedNotificationCuDto, CreatedBy, cancellationToken);
            }
            return new ApiResult<TicketReferListDto>(true, ApiResultStatusCode.Success, null, null);
        }
    }
}
