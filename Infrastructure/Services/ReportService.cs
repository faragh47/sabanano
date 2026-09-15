using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Common;
using Common;
using Data.Contracts;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.SharedModels;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using X.PagedList;

namespace Services.Services
{
    public class ReportService<TListDto, TSearchDto, TEntity, TKey> : IReportService<TListDto, TSearchDto, TEntity, TKey>
        where TListDto : BaseDto<TListDto, TEntity, TKey>, new()
        where TEntity : BaseAuditableEntity<TKey>, new()
        where TSearchDto : BaseSearchDto//, IHaveCustomExpression<TEntity, TSearchDto, TKey>
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository<TEntity> Repository;

        public ReportService(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<ApiResult<IPagedList<TListDto>>> Get(TSearchDto searchDto,
            CancellationToken cancellationToken)
        {
            //var expression = searchDto.GenerateExpression(searchDto);
            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<TListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var result = await Repository.TableNoTracking
                .OrderByDescending(src => src.Id)
               // .Where(expression)
                .ProjectTo<TListDto>(Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<TListDto>>(false, ApiResultStatusCode.NotFound, null);


            return new ApiResult<IPagedList<TListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }


        public virtual async Task<ApiResult<TListDto>> Get(TKey id, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TListDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (dto == null)
                return new ApiResult<TListDto>(false, ApiResultStatusCode.NotFound, null);

            return dto;
        }

        public virtual async Task<ApiResult<List<TListDto>>> GetAll(TSearchDto searchDto, CancellationToken cancellationToken)
        {
            //var expression = searchDto.GenerateExpression(searchDto);
            var result = await Repository.TableNoTracking
                //.Where(expression)
                .ProjectTo<TListDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            if (result.Count == 0)
                return new ApiResult<List<TListDto>>(false, ApiResultStatusCode.NotFound, null);
            return new ApiResult<List<TListDto>>(true, ApiResultStatusCode.Success, result);
        }
    }
}
