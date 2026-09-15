using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.SharedModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.IServices.V2;
using X.PagedList;

namespace Services.Services.V2
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<AccGroupRole> _GroupRoleRepository;
        private readonly IRepository<AccGroupUser> _GroupUserRepository;
        private readonly IRepository<AccGroup> _GroupRepository;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<AccRole> _RoleManager;
        private readonly IMapper _Mapper;

        public GroupService(IRepository<AccGroupRole> groupRoleRepository,
            IRepository<AccGroupUser> groupUserRepository,
            IRepository<AccGroup> groupRepository, UserManager<ApplicationUser> userManager,
            RoleManager<AccRole> roleManager, IMapper mapper)
        {
            this._GroupRoleRepository = groupRoleRepository;
            this._GroupUserRepository = groupUserRepository;
            this._GroupRepository = groupRepository;
            this._UserManager = userManager;
            this._RoleManager = roleManager;
            this._Mapper = mapper;
        }

        public async Task<ApiResult<GroupListDto>> AddRoleToGroup(GroupSearchDto.AddRoleToGroupDto dto, long CreatorId, CancellationToken cancellationToken)
        {
            var existingGroup =
                await _GroupRepository.TableNoTracking.
                ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).FirstOrDefaultAsync(src => src.Id == dto.GroupId, cancellationToken);

            if (existingGroup is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var existingRole = await _RoleManager.FindByIdAsync(dto.RoleId.ToString());

            if (existingRole is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);


            var existingGroupRole = await _GroupRoleRepository.TableNoTracking.FirstOrDefaultAsync(
                x => x.GroupId == dto.GroupId && x.RoleId == dto.RoleId, cancellationToken);

            if (existingGroupRole is not null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.ExistRoleInGroup, null);

            var entity = new AccGroupRole()
            {
                RoleId = dto.RoleId,
                GroupId = dto.GroupId
            };// dto.ToEntity(_Mapper);
            //entity.AddCreator<AccGroupRole, long>(CreatorId);
            await _GroupRoleRepository.AddAsync(entity, cancellationToken);

            if (entity.Id is 0)
            {
                throw new AppException(ApiResultStatusCode.InsertFailed);
            }

            var groupUsers = _GroupUserRepository.TableNoTracking.Where(x => x.GroupId == dto.GroupId);

            foreach (var groupUser in groupUsers)
            {
                var user = await _UserManager.FindByIdAsync(groupUser.UserId.ToString());

                if (user is null) throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

                bool existRoleInUser = await _UserManager.IsInRoleAsync(user, existingRole.Name);

                if (!existRoleInUser)
                {
                    var result = await _UserManager.AddToRoleAsync(user, existingRole.Name);
                    // var result = await _identityService.AddToRoleDbAndCache(user, existingRole.Name);
                    if (!result.Succeeded)
                        throw new BadRequestException(ApiResultStatusCode.InsertFailed.ToDisplay());
                }

            }

            return existingGroup;
        }

        public async Task<ApiResult<GroupListDto>> AddUserToGroup(GroupSearchDto.AddUserToGroupDto dto, long CreatorId, CancellationToken cancellationToken)
        {
            var existingGroup =
                 await _GroupRepository.TableNoTracking.
                 ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).FirstOrDefaultAsync(src => src.Id == dto.GroupId, cancellationToken);

            if (existingGroup is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var existingUser = await _UserManager.FindByIdAsync(dto.UserId.ToString());
            if (existingUser is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var existingGroupUser = await _GroupUserRepository.TableNoTracking.FirstOrDefaultAsync(
                x => x.GroupId == dto.GroupId && x.UserId == dto.UserId, cancellationToken);

            if (existingGroupUser is not null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.ExistUserInGroup, null);

            var entity = new AccGroupUser
            {
                GroupId = dto.GroupId,
                UserId = dto.UserId
            };
            //entity.AddCreator<AccGroupUser, long>(CreatorId);
            await _GroupUserRepository.AddAsync(entity, cancellationToken);

            if (entity.Id is 0)
            {
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.InsertFailed, null);
            }

            var groupRoles = await _GroupRoleRepository.TableNoTracking.Where(x => x.GroupId == dto.GroupId).ToListAsync();

            foreach (var groupRole in groupRoles)
            {
                var role = await _RoleManager.FindByIdAsync(groupRole.RoleId.ToString());

                bool existRoleInUser = await _UserManager.IsInRoleAsync(existingUser, role.Name);

                if (!existRoleInUser)
                {
                     var result = await _UserManager.AddToRoleAsync(existingUser, role.Name);
                    //var result = await _identityService.AddToRoleDbAndCache(existingUser, role.Name);
                    if (!result.Succeeded)
                        return new ApiResult<GroupListDto>(false, ApiResultStatusCode.InsertFailed, null);
                }
            }
            return existingGroup;
        }

        public async Task<ApiResult<GroupListDto>> Create(GroupCuDto dto, CancellationToken cancellationToken)
        {
            var exists = await _GroupRepository.TableNoTracking.AnyAsync(x => x.Title == dto.Title);
            if (exists is true)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NameIsExists, null);

            var entity = dto.ToEntity(_Mapper);

            await _GroupRepository.AddAsync(entity, cancellationToken);

            var group = _GroupRepository.TableNoTracking
                .Where(x => x.Id == dto.Id).ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).FirstOrDefault();
            return group;

        }

        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var exists = await _GroupRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
            await _GroupRepository.DeleteAsync(exists, cancellationToken);
            return new ApiResult<RoleListDto>(true, ApiResultStatusCode.Success, null);
        }

        public async Task<ApiResult<IPagedList<GroupListDto>>> Get(GroupSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<GroupListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);
            var result = await _GroupRepository.TableNoTracking
                .Where(expression).ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<GroupListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<GroupListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<IList<GroupListDto>>> GetAll(GroupSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            var result = await _GroupRepository.TableNoTracking
                .Where(expression).ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IList<GroupListDto>>(false, ApiResultStatusCode.NotFound, null, ApiResultStatusCode.NotFound.ToDisplay());

            return new ApiResult<IList<GroupListDto>>(true, ApiResultStatusCode.Success, result, ApiResultStatusCode.Success.ToDisplay());

        }

        public async Task<ApiResult<IPagedList<GroupListDto>>> GetGroupsByRoleId(GroupSearchDto dto
            , CancellationToken cancellationToken)
        {
            if (dto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<GroupListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var groups = from groupRole in _GroupRoleRepository.TableNoTracking
                         join groupItem in _GroupRepository.TableNoTracking on groupRole.GroupId equals groupItem.Id
                         where groupRole.RoleId == dto.RoleId
                         select groupItem;

            var expression = dto.GenerateExpression(dto);

            var result =
                await groups.
                Where(expression).
                ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);

            if (result is { Count: 0 })
                return new ApiResult<IPagedList<GroupListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<GroupListDto>>(true, ApiResultStatusCode.Success, result, null,
            result.TotalItemCount,
            result.PageNumber, result.PageCount);
        }
        public async Task<ApiResult<IPagedList<GroupListDto>>> GetGroupsByUserId(GroupSearchDto dto, CancellationToken cancellationToken)
        {
            if (dto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<GroupListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var groups = from groupUser in _GroupUserRepository.TableNoTracking
                         join groupItem in _GroupRepository.TableNoTracking on groupUser.GroupId equals groupItem.Id
                         where groupUser.UserId == dto.UserId
                         select groupItem;

            var expression = dto.GenerateExpression(dto);

            var result =
                    await groups.
                    Where(expression).
                    ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);

            if (result is { Count: 0 })
                return new ApiResult<IPagedList<GroupListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<GroupListDto>>(true, ApiResultStatusCode.Success, result, null,
            result.TotalItemCount,
            result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<GroupListDto>> RemoveRoleFromGroup(GroupSearchDto.AddRoleToGroupDto dto, CancellationToken cancellationToken)
        {
            var existingGroup =
                await _GroupRepository.TableNoTracking.
                ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).FirstOrDefaultAsync(src => src.Id == dto.GroupId, cancellationToken);

            if (existingGroup is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var existingRole = await _RoleManager.FindByIdAsync(dto.RoleId.ToString());

            if (existingRole is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var entity = await _GroupRoleRepository.TableNoTracking.FirstOrDefaultAsync(
                 x => x.GroupId == dto.GroupId && x.RoleId == dto.RoleId, cancellationToken);

            if (entity is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            await _GroupRoleRepository.DeleteAsync(entity, cancellationToken);

            var groupUsers = _GroupUserRepository.TableNoTracking.Where(x => x.GroupId == dto.GroupId);

            foreach (var groupUser in groupUsers)
            {
                var user = await _UserManager.FindByIdAsync(groupUser.UserId.ToString());

                bool existRoleInUser = await _UserManager.IsInRoleAsync(user, existingRole.Name);

            }
            return existingGroup;
        }

        public async Task<ApiResult<GroupListDto>> RemoveUserFromGroup(GroupSearchDto.AddUserToGroupDto dto, CancellationToken cancellationToken)
        {
            var existingGroup =
                 await _GroupRepository.TableNoTracking.
                 ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).FirstOrDefaultAsync(src => src.Id == dto.GroupId, cancellationToken);

            if (existingGroup is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var existingUser = await _UserManager.FindByIdAsync(dto.UserId.ToString());
            if (existingUser is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            var entity = await _GroupUserRepository.TableNoTracking.FirstOrDefaultAsync(
                 x => x.GroupId == dto.GroupId && x.UserId == dto.UserId, cancellationToken);

            if (entity is null)
                return new ApiResult<GroupListDto>(false, ApiResultStatusCode.NotFound, null);

            await _GroupUserRepository.DeleteAsync(entity, cancellationToken);

            var groupRoles = await _GroupRoleRepository.TableNoTracking.Where(x => x.GroupId == dto.GroupId).ToListAsync();

            foreach (var groupRole in groupRoles)
            {
                var role = await _RoleManager.FindByIdAsync(groupRole.RoleId.ToString());

                bool existRoleInUser = await _UserManager.IsInRoleAsync(existingUser, role.Name);
            }
            return existingGroup;
        }

        public async Task<ApiResult<GroupListDto>> Update(GroupCuDto dto, CancellationToken cancellationToken)
        {
            var model = await _GroupRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == dto.Id);

            await _GroupRepository.UpdateAsync(model, cancellationToken);

            var role = _GroupRepository.TableNoTracking
                .Where(x => x.Id == dto.Id).ProjectTo<GroupListDto>(_Mapper.ConfigurationProvider).FirstOrDefault();
            return role;
        }
    }
}
