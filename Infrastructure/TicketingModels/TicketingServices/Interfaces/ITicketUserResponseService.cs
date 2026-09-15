using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using Common;
using Entities.DatabaseModels.TicketingModels;
using Services.IServices;

namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketUserResponseService : ICrudService<TicketUserResponseCuDto, TicketUserResponseListDto, TicketUserResponseSearchDto, TicketUserResponse, long>, IScopedDependency
    {
    }
}
