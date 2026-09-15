using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;
using static Common.Utilities.GlobalEnums;

namespace Services.Services.V2
{
    public class TicketCategoryService : CrudService<TicketCategoryCuDto, TicketCategoryListDto, TicketCategorySearchDto, TicketCategory, int>, ITicketCategoryService
    {
        private readonly IRepository<TicketCategory> _TicketCategoryRepository;
        private readonly IRepository<Ticket> _TicketRepository;
        private readonly IMapper _Mapper;
        public TicketCategoryService(IRepository<TicketCategory> repository,
            IRepository<TicketCategory> ticketCategoryRepository,
            IRepository<Ticket> ticketRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _Mapper = mapper;
            _TicketCategoryRepository = ticketCategoryRepository;
            _TicketRepository = ticketRepository;
        }
        public override async Task<ApiResult<TicketCategoryListDto>> Create(TicketCategoryCuDto dto, long creatorId, CancellationToken cancellationToken)
        {
            var existing = _TicketCategoryRepository.TableNoTracking.Where(x=>x.Title == dto.Title).FirstOrDefault();
            if (existing != null)
            {
                return new ApiResult<TicketCategoryListDto>(false, ApiResultStatusCode.BadRequest, null, "ثبت رکورد تکراری");
            }
            return await base.Create(dto, creatorId, cancellationToken);
        }
        public async Task<ApiResult<List<TicketCategoryBriefListDto>>> GetAll(TicketCategorySearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
          //  expression = expression.AndExpression(src => searchDto.ParentId == null);

            var result = await Repository.TableNoTracking
                .OrderBy(src => src.Id)
                .Where(expression).ProjectTo<TicketCategoryBriefListDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            if (result.Count == 0)
                return new ApiResult<List<TicketCategoryBriefListDto>>(false, ApiResultStatusCode.NotFound, null);
            return new ApiResult<List<TicketCategoryBriefListDto>>(true, ApiResultStatusCode.Success, result);
        }
        public override async Task<ApiResult> Delete(int id, long modifierId, CancellationToken cancellationToken)
        {
            var existingTicket = _TicketRepository.TableNoTracking.Where(x => x.TicketCategoryId == id).FirstOrDefault();
            if (existingTicket is not null)
            {
                return new ApiResult<TicketCategoryListDto>(false, ApiResultStatusCode.UpdateFailed, null, "امکان حذف این رکورد وجود ندارد زیرا توسط آن تیکت ایجاد شده است.");
            }

           // var existingChild = Repository.TableNoTracking.Where(x => x.ParentId == id).FirstOrDefault();
         

            var existingCategory = Repository.TableNoTracking.Where(x => x.Id == id).FirstOrDefault();
            if (existingCategory is null)
            {
                return new ApiResult<TicketCategoryListDto>(false, ApiResultStatusCode.UpdateFailed, null, "موضوع مورد نظر یافت نشد.");
            }

            var model = await Repository.GetByIdAsync(cancellationToken, id);
            try
            {
                await Repository.DeleteAsync(model, cancellationToken);

                return new ApiResult<TicketCategoryListDto>(true, ApiResultStatusCode.Success, null);
            }
            catch (Exception)
            {
                model.IsActive = false;
                await Repository.UpdateAsync(model, cancellationToken);
                return new ApiResult<TicketCategoryListDto>(true, ApiResultStatusCode.Success, null);
            }
        }
    }
}
