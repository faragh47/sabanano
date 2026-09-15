using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using Common;
using Entities.DatabaseModels.TicketingModels;
using Services.IServices;

namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketStatusHistoryService : ICrudService<TicketStatusHistoryCuDto, TicketStatusHistoryListDto, TicketStatusHistorySearchDto, TicketStatusHistory, long>, IScopedDependency
    {
    }
}
