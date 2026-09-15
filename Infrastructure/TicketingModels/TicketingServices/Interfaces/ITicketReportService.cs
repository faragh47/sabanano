using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using Common;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Services.IServices;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;


namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketReportService : ICrudService<TicketCuDto, TicketReportListDto, TicketReportSearchDto, Ticket, long>, IScopedDependency
    {
        Task<ApiResult<IPagedList<TicketReportListDto>>> GetTicketList(long userId, TicketReportSearchDto searchDto, CancellationToken cancellationToken);
    }
}
