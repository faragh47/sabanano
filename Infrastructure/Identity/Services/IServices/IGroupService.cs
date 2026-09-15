using System.Threading;
using System.Threading.Tasks;
using Common;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.SharedModels;
using X.PagedList;

namespace Services.IServices.V2
{
    public interface IGroupService: IScopedDependency
    {
        Task<ApiResult<GroupListDto>> Create(GroupCuDto roleCuDto, CancellationToken cancellationToken);
        Task<ApiResult<GroupListDto>> Update(GroupCuDto roleCuDto, CancellationToken cancellationToken);
        Task<ApiResult> Delete(int id, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<GroupListDto>>> Get(GroupSearchDto searchDto, CancellationToken cancellationToken);
        Task<ApiResult<IList<GroupListDto>>> GetAll(GroupSearchDto searchDto, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<GroupListDto>>> GetGroupsByRoleId(GroupSearchDto dto, CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<GroupListDto>>> GetGroupsByUserId(GroupSearchDto dto, CancellationToken cancellationToken);
        Task<ApiResult<GroupListDto>> AddRoleToGroup(GroupSearchDto.AddRoleToGroupDto dto,long CreatorId, CancellationToken cancellationToken);
        Task<ApiResult<GroupListDto>> RemoveRoleFromGroup(GroupSearchDto.AddRoleToGroupDto dto, CancellationToken cancellationToken);
        Task<ApiResult<GroupListDto>> AddUserToGroup(GroupSearchDto.AddUserToGroupDto dto, long CreatorId, CancellationToken cancellationToken);
        Task<ApiResult<GroupListDto>> RemoveUserFromGroup(GroupSearchDto.AddUserToGroupDto dto, CancellationToken cancellationToken);
    }
}
