using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing;
using Common;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Services.IServices.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Services.Services.V2.Ticketing
{
    public class TicketReportService : CrudService<TicketCuDto, TicketReportListDto, TicketReportSearchDto, Ticket, long>, ITicketReportService
    {
        private readonly IRepository<TicketCategory> _categoriesRepository;
        private readonly IRoleService _roleService;
        private readonly IUsersService _usersService;

        public TicketReportService(IRepository<Ticket> repository,
            IRepository<TicketCategory> CategoriesRepository,
            IRoleService roleService,
            IUsersService usersService,
            IMapper mapper) : base(repository, mapper)
        {
            _categoriesRepository = CategoriesRepository;
            _roleService = roleService;
            _usersService = usersService;
        }

        public async Task<ApiResult<IPagedList<TicketReportListDto>>> GetTicketList(long userId, TicketReportSearchDto searchDto, CancellationToken cancellationToken)
        {

            var hasAllTicketsRole = await _usersService.UserIsInRole(userId, "Ticket-AllTickets", cancellationToken);
            var hasSupeAdminRole = await _usersService.UserIsInRole(userId, "SuperAdmin", cancellationToken);

            if (hasAllTicketsRole.IsSuccess || hasSupeAdminRole.IsSuccess)
            {
                return await Get(searchDto, cancellationToken);
            }

            //var ticketingCategories = await _categoriesRepository.TableNoTracking.ToLookup(p => p.RoleId).Select(p => p.First()).ToListAsync(cancellationToken);
            
            //var userTicketingRoles = new LinkedList<long>();
            //foreach (var category in ticketingCategories)
            //{
            //   // var isInRole = await _usersService.UserIsInRoleByRoleId(userId, category.RoleId, cancellationToken);
            //    //if(isInRole.Data.Result)
            //    //    userTicketingRoles.AddLast(category.FRoleId);
            //}
            //long[] userTicketingRolesIds = new long[userTicketingRoles.Count];
            //userTicketingRoles.CopyTo(userTicketingRolesIds, 0);

            //if (userTicketingRoles.Count() == 0)
            //    return new ApiResult<IPagedList<TicketReportListDto>>(false, ApiResultStatusCode.NotFound, null, ApiResultStatusCode.NotFound.ToDisplay());

            //searchDto.DesiredRoleIds = userTicketingRolesIds;
            var expression = searchDto.GenerateExpression(searchDto);

            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketReportListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                .ThenBy(src => src.TicketStatusId)
                .ThenByDescending(src => src.Created)
                .Where(expression)
                .ProjectTo<TicketReportListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketReportListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketReportListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public override async Task<ApiResult<IPagedList<TicketReportListDto>>> Get(TicketReportSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TicketReportListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.IsActive)
                .ThenBy(src => src.TicketStatusId)
                .ThenByDescending(src => src.Created)
                .Where(expression).ProjectTo<TicketReportListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);
            if (result.Count == 0)
                return new ApiResult<IPagedList<TicketReportListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<TicketReportListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }
    }
}
