using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using Common;
using Entities.DatabaseModels.TicketingModels;
using Services.IServices;

namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketReferService : ICrudService<TicketReferCuDto, TicketReferListDto, TicketReferSearchDto, TicketReferHistory, long>, IScopedDependency
    {
    }
}
