using AutoMapper;
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
using static Common.Utilities.GlobalEnums;

namespace Services.Services.V2
{
    public class TicketReferService : CrudService<TicketReferCuDto, TicketReferListDto, TicketReferSearchDto, TicketReferHistory, long>, ITicketReferService
    {
        private readonly IRepository<Ticket> _TicketRepository;
        private readonly IRepository<TicketStatusHistory> _TicketStatusHistoryRepository;
        private readonly IMapper _Mapper;
        public TicketReferService(IRepository<TicketReferHistory> repository,
            IRepository<Ticket> ticketRepository,
            IRepository<TicketStatusHistory> ticketStatusHistoryRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _TicketRepository = ticketRepository;
            _TicketStatusHistoryRepository = ticketStatusHistoryRepository;
            _Mapper = mapper;
        }

        public override async Task<ApiResult<TicketReferListDto>>   Create(TicketReferCuDto dto, long creatorId, CancellationToken cancellationToken)
        {

            var tikTicketReferCuDto = new TicketReferCuDto
            {
                TicketId = dto.TicketId,
                FromUserId = dto.FromUserId,
                ToUserId = dto.ToUserId,
            };

            await base.Create(tikTicketReferCuDto, 2, cancellationToken);

            var existingRecord = await _TicketRepository.TableNoTracking.SingleOrDefaultAsync(p => p.Id.Equals(dto.TicketId), cancellationToken);
            existingRecord.TicketStatusId = (int)GlobalEnums.TicketStatus.InProgress;
            existingRecord.ReferedUserId = dto.ToUserId;
            await _TicketRepository.UpdateAsync(existingRecord, cancellationToken);
            var ticketStatusHistory = new TicketStatusHistory
            {
                TicketId = dto.TicketId,
                StatusId = existingRecord.TicketStatusId,
            };
            await _TicketStatusHistoryRepository.AddAsync(ticketStatusHistory, cancellationToken);

            return new ApiResult<TicketReferListDto>(true, ApiResultStatusCode.Success, null, null);
        }

    }
}
