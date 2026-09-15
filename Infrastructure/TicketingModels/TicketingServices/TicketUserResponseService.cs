using AutoMapper;
using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Services.IServices.V2;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.V2
{
    public class TicketUserResponseService : CrudService<TicketUserResponseCuDto, TicketUserResponseListDto, TicketUserResponseSearchDto, TicketUserResponse, long>, ITicketUserResponseService
    {
        private readonly IMapper _Mapper;
        public TicketUserResponseService(IRepository<TicketUserResponse> repository,
            IMapper mapper) : base(repository, mapper)
        {
            _Mapper = mapper;
        }
    }
}
