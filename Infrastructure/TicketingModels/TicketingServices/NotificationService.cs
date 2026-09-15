using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using Common;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.NotificationModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Services.IServices.V2.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;
using static Common.Utilities.GlobalEnums;

namespace Services.Services.V2
{
    public class NotificationService : CrudService<NotificationCuDto, NotificationListDto, NotificationSearchDto, Notification, long>, INotificationService
    {
        private readonly IRepository<Person> _PersonRepository;
        private readonly IRepository<ApplicationUser> _UserRepository;
        private readonly IRepository<PersonMobileNumber> _PersonMobileNumberRepository;
       // IHubContext<TicketHub, ITicket> _hubContext;
        private readonly IMapper _Mapper;

        public NotificationService(IRepository<Notification> repository,
            IRepository<Person> PersonRepository,
            IRepository<ApplicationUser> UserRepository,
          //  IHubContext<TicketHub, ITicket> hubContext,
            IRepository<PersonMobileNumber> personMobileNumberRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _PersonRepository = PersonRepository;
            _UserRepository = UserRepository;
           // _hubContext = hubContext;
            _PersonMobileNumberRepository = personMobileNumberRepository;
            _Mapper = mapper;
        }

        public override async Task<ApiResult<NotificationListDto>> Create(NotificationCuDto dto, long creatorId, CancellationToken cancellationToken)
        {
            var existingUser = await _UserRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id == dto.PersonId);
            var existingPerson = await _PersonRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id == existingUser.PersonId);
            ApiResult<NotificationListDto> result = new ApiResult<NotificationListDto>(false, ApiResultStatusCode.BadRequest, null, null);
            if (existingUser is not null)
            {
                dto.UserId = existingUser.Id;
                dto.PersonId = existingPerson.Id;
            }

            DateTime today = DateTime.Today;

            //if (dto.SendDate < today)
            //{
            //    return new ApiResult<NotificationListDto>(false, ApiResultStatusCode.InvalidInputData, null, "تاریخ ارسال نامعتبر است.");
            //}

            if (dto.UserId == null)
            {
                if (dto.PersonId == null)
                {
                    return new ApiResult<NotificationListDto>(false, ApiResultStatusCode.InvalidInputData, null, "ورودی نامعتبر");
                }
                else
                {
                    // PersonId Exists
                    var existingMobileNumber = await _PersonMobileNumberRepository.TableNoTracking.FirstOrDefaultAsync(x => x.PersonId == dto.PersonId);

                    if (dto.SendSMS == false)
                    {
                        return new ApiResult<NotificationListDto>(false, ApiResultStatusCode.InvalidInputData, null, "با این شرایط امکان ثبت رکورد وجود ندارد.");
                    }

                    if (existingMobileNumber.MobileNumber != null)
                    {
                        var notificationCuDto = new NotificationCuDto()
                        {
                            Title = dto.Title,
                            Description = dto.Description,
                            PersonId = dto.PersonId,
                            FrontendRouteId = dto.FrontendRouteId,
                            ContextId = dto.ContextId,
                            SendSMS = dto.SendSMS,
                            SendInApp = dto.SendInApp,
                            SendDate = dto.SendDate,
                            Seen = false,
                        };

                        result = await base.Create(notificationCuDto, creatorId, cancellationToken);
                    }
                }
            }
            else
            {
                if (dto.PersonId == null)
                {
                    // UserId Exists
                    var notificationCuDto = new NotificationCuDto()
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        UserId = dto.UserId,
                        FrontendRouteId= dto.FrontendRouteId,
                        ContextId= dto.ContextId,
                        SendSMS = dto.SendSMS,
                        SendInApp = dto.SendInApp,
                        SendDate = dto.SendDate,
                        Seen = false,
                    };

                    result = await base.Create(notificationCuDto, creatorId, cancellationToken);
                }
                else
                {
                    // UserId and PersonId both Exist
                    var notificationCuDto = new NotificationCuDto()
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        UserId = dto.UserId,
                        PersonId = dto.PersonId,
                        FrontendRouteId = dto.FrontendRouteId,
                        ContextId = dto.ContextId,
                        SendSMS = dto.SendSMS,
                        SendInApp = dto.SendInApp,
                        SendDate = dto.SendDate,
                        Seen = false,
                    };

                    result = await base.Create(notificationCuDto, creatorId, cancellationToken);
                }
            }

            //if (result.IsSuccess && dto.UserId is not null)
            //{
            //   await _hubContext.Clients.Group(result.Data.UserId.ToString()).SendNotification(result);
            //}

            return result;
        }

        public override async Task<ApiResult<NotificationListDto>> Update(NotificationCuDto dto, long modifierId, CancellationToken cancellationToken)
        {
            //var existingRecord = Repository.TableNoTracking.First();
            var existingRecord = await Repository.TableNoTracking.SingleOrDefaultAsync(x => x.Id == dto.Id);

            //DateTime today = DateTime.Today;

            //if (dto.SendDate < today)
            //{
            //    return new ApiResult<NotificationListDto>(false, ApiResultStatusCode.InvalidInputData, null, "تاریخ ارسال نامعتبر است.");
            //}
            existingRecord.Seen = true;
            await Repository.UpdateAsync(existingRecord, cancellationToken);

            return new ApiResult<NotificationListDto>(true, ApiResultStatusCode.Success, null);
        }

        public override async Task<ApiResult<IPagedList<NotificationListDto>>> Get(NotificationSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            var result = await Repository.TableNoTracking
                .Where(expression)
                .OrderByDescending(src => src.Created)
                .ProjectTo<NotificationListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);
            if (result.Count == 0)
                return new ApiResult<IPagedList<NotificationListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<NotificationListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<IPagedList<NotificationListDto>>> GetMyNotifications(NotificationSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            var result = await Repository.TableNoTracking
                .Where(expression)
                .OrderBy(src => src.Seen)
                .ThenByDescending(src => src.Created)
                .ProjectTo<NotificationListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);
            if (result.Count == 0)
                return new ApiResult<IPagedList<NotificationListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<NotificationListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<List<NotificationListDto>>> GetAllMyNotifications(NotificationSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            var result = await Repository.TableNoTracking
                .Where(expression)
                .OrderBy(src => src.Seen)
                .ThenByDescending(src => src.Created)
                .ProjectTo<NotificationListDto>(Mapper.ConfigurationProvider)
                .ToListAsync();

            if (result.Count == 0)
                return new ApiResult<List<NotificationListDto>>(false, ApiResultStatusCode.NotFound, null);

            return result;
        }
    }
}
