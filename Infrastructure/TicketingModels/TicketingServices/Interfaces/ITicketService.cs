using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using Common;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketService : IScopedDependency
    //ICrudService<TicketCuDto, TicketListDto, TicketSearchDto, TikTicket, long>, IScopedDependency
    {
        Task<ApiResult<TicketListDto>> CreateTicket(TicketCuDto model, List<IFormFile> files, long creatorId, string folderPath, CancellationToken cancellationToken);
        Task<ApiResult<TicketListDto>> CreateTicketResponse(TicketUserResponseCuDto model, List<IFormFile> files, long creatorId, string folderPath, CancellationToken cancellationToken);
        Task<ApiResult<TicketReferListDto>> CreateTicketRefer(TicketReferCuDto model, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<TicketListDto>>> GetTicketList(TicketSearchDto model, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<TicketBriefListDto>>> TicketArchivedList(TicketSearchDto model, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<TicketBriefListDto>>> GetBriefTicketList(TicketSearchDto model, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<TicketStatusHistoryListDto>> ChangeTicketStatus(TicketStatusHistoryCuDto searchDto, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<TicketListDto>>> GetAll(TicketSearchDto model, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<TicketBriefListDto>>> GetAllBrief(TicketSearchDto model, long userId, CancellationToken cancellationToken);
        Task<ApiResult<TicketListDto>> Get(long id, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<TicketBriefListDto>>> GetBrief(TicketSearchDto searchDto, CancellationToken cancellationToken);
        //Task<ApiResult<TicketDashboardListDto>> TicketDashboard(TicketSearchDto searchDto, CancellationToken cancellationToken);
        //Task<ApiResult<List<TicketCompareDashboardListDto>>> TicketCompareDashboard(TicketSearchDto searchDto, CancellationToken cancellationToken);
        Task<ApiResult<TicketReportDto>> TicketReport(TicketSearchDto model, long creatorId, CancellationToken cancellationToken);
        //Task InitializeAutomaticTicketDashboard(CancellationToken cancellationToken);
        Task AutomaticTicketClosing(CancellationToken cancellationToken);
        Task<ApiResult<TicketListDto>> ChangeTicketCategory(TicketCuDto model, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<TicketReferListDto>> ReferClosedTicket(TicketReferCuDto model, long creatorId, CancellationToken cancellationToken);
    }
}
