using System.Threading;
using System.Threading.Tasks;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.SharedModels;
using X.PagedList;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Common;

namespace Services.IServices
{
    public interface ICrudService<TDto, TListDto, TSearchDto, TEntity, TKey>:IReportService<TListDto, TSearchDto, TEntity, TKey>
        where TDto : BaseDto<TDto, TEntity, TKey>, new()
        where TListDto : BaseDto<TListDto, TEntity, TKey>, new()
        where TEntity : BaseAuditableEntity<TKey>, new()
        where TSearchDto : BaseSearchDto//, IHaveCustomExpression<TEntity, TSearchDto, TKey>
 
    {
        Task<ApiResult<TListDto>> Create(TDto dto, long creatorId, CancellationToken cancellationToken);
        Task<ApiResult<TListDto>> Update(TDto dto, long modifierId, CancellationToken cancellationToken);
        Task<ApiResult<TListDto>> SoftUpdate(TDto dto, long modifierId, CancellationToken cancellationToken);
        Task<ApiResult> Delete(TKey id, long modifierId, CancellationToken cancellationToken);
        Task<ApiResult> SoftDelete(TKey id, long modifierId, CancellationToken cancellationToken);
    }
}
