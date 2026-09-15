using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using Common;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.SharedModels;
using X.PagedList;

namespace Services.IServices.V2
{
    public interface IUsersService: IScopedDependency
    {
        Task<ApiResult<IPagedList<ApplicationUserListDto>>> GetUsersByRoleId(ApplicationUserSearchDto dto,CancellationToken cancellationToken);
        Task<ApiResult<ApplicationUserListDto>> CreateUser(ApplicationUserCuDto user, long UserId ,CancellationToken cancellationToken);
        Task<ApplicationUserListDto> GetUserById(long userId, CancellationToken cancellationToken);
        Task<AccessToken> Login(string userName, string password, CancellationToken cancellationToken);
        Task<ApiResult<ApplicationUserListDto>> AddRoleToUser(long userId, long roleId, CancellationToken cancellationToken);
        Task<ApiResult<List<ApplicationUserListDto>>> GetUsersByNationalId(ApplicationUserSearchDto user, CancellationToken cancellationToken);

        Task<ApiResult<IPagedList<ApplicationUserListDto>>> Get(ApplicationUserSearchDto searchDto,
            CancellationToken cancellationToken);
        Task<ApiResult<IPagedList<ApplicationUserListDto>>> GetAssignTicket(ApplicationUserSearchDto searchDto,
            CancellationToken cancellationToken);

        Task<ApiResult<ApplicationUserListDto>> ChangePassword(
            ApplicationUserSearchDto.ApplicationUserChangePasswordDto dto, bool isAdmin,
            CancellationToken cancellationToken);

        Task<ApplicationUserListDto> UpdateUser(ApplicationUserCuDto user, long UserId, CancellationToken cancellationToken);

        Task<ApiResult<ApplicationUserListDto>> RemoveRoleFromUser(long userId, long roleId, CancellationToken cancellationToken);

        Task<ApiResult<IPagedList<ApplicationUserListDto>>> GetUsersByGroupId(ApplicationUserSearchDto dto, CancellationToken cancellationToken);

        Task<ApiResult> UserIsInRole(long userId, string roleName, CancellationToken cancellationToken);
        Task<ApiResult<ApplicationUserListDto>> SetActiveUser(ApplicationUserActiveDto dto, long UserId, CancellationToken cancellationToken);
        Task<ApiResult<PeopleBriefDto>> GetPeopleByToken( long UserId, CancellationToken cancellationToken);
    }
}
