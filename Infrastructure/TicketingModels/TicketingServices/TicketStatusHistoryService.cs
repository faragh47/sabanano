using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.EntityFrameworkCore;
using Services.IServices.V2;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.V2
{
    public class TicketStatusHistoryService : CrudService<TicketStatusHistoryCuDto, TicketStatusHistoryListDto, TicketStatusHistorySearchDto, TicketStatusHistory, long>, ITicketStatusHistoryService
    {
        private readonly IRepository<Ticket> _TicketRepository;
        private readonly IMapper _Mapper;
        private readonly IRepository<TicketStatusHistory> _TicketStatusHistoryRepository;
        public TicketStatusHistoryService(IRepository<TicketStatusHistory> repository,
        IRepository<Ticket> ticketRepository,
        IRepository<TicketStatusHistory> ticketStatusHistoryRepository,
        IMapper mapper) : base(repository, mapper)
        {
            _TicketRepository = ticketRepository;
            _Mapper = mapper;
            _TicketStatusHistoryRepository = ticketStatusHistoryRepository;
        }


        //The rest may result an EF tracking error

        public override async Task<ApiResult<TicketStatusHistoryListDto>> Create(TicketStatusHistoryCuDto dto, long creatorId, CancellationToken cancellationToken)
        {

            var tikTicketStatusHistoryCuDto = new TicketStatusHistoryCuDto
            {
                TicketId = dto.TicketId,
                StatusId = dto.StatusId,
            };

            // await base.Create(tikTicketStatusHistoryCuDto, 2, cancellationToken);

            //var StatusHistory = await _TicketStatusHistoryService.Create(ticketStatusHistoryCuDto, creatorId, cancellationToken);


            var entity = dto.ToEntity(_Mapper);

        

            await _TicketStatusHistoryRepository.AddAsync(entity, cancellationToken);


            var existingRecord = await _TicketRepository.TableNoTracking.SingleOrDefaultAsync(p => p.Id.Equals(dto.TicketId), cancellationToken);
            existingRecord.TicketStatusId = dto.StatusId;

            await _TicketRepository.UpdateAsync(existingRecord, cancellationToken);

            return new ApiResult<TicketStatusHistoryListDto>(true, ApiResultStatusCode.Success, null, null);
        }

    }
}
