using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing;
using Data.Contracts;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.V2.Ticketing
{
    //public class TicketDashboardStatService : CrudService<TicketDashboardStatCuDto, TicketDashboardStatListDto, TicketDashboardStatSearchDto, TicketDashboardStat, long>, ITicketDashboardStatService
    //{
    //    public TicketDashboardStatService(IRepository<TicketDashboardStat> repository,
    //        IMapper mapper) : base(repository, mapper)
    //    {
    //    }
    //    public override async Task<ApiResult<TicketDashboardStatListDto>> Create(TicketDashboardStatCuDto dto, long creatorId, CancellationToken cancellationToken)
    //    {
    //        var model = dto.ToEntity(Mapper);

    //        foreach (var item in model.TicketDashboardCategoryStat)
    //        {
    //            item.AddCreator<TicketDashboardCategoryStat, long>(creatorId, dto.CreationDate);
    //        }

    //        await Repository.AddAsync(model, creatorId, cancellationToken);

    //        var resultDto = await Repository.TableNoTracking.ProjectTo<TicketDashboardStatListDto>(Mapper.ConfigurationProvider)
    //            .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

    //        return resultDto;
    //    }
    //}
}
