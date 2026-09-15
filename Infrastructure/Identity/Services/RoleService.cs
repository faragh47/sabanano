using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Infrastructure.Identity;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.SharedModels;
using Microsoft.AspNetCore.Identity;
using Services.IServices.V2;
using X.PagedList;

namespace Services.Services.V2
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AccRole> _roleManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IMapper _Mapper;
        private readonly IRepository<AccGroupRole> _GroupRoleRepository;
        private readonly IRepository<AccRole> _RoleRepository;
        private readonly IRepository<ApplicationUser> _UserRepository;

        public RoleService(RoleManager<AccRole> roleManager,
            UserManager<ApplicationUser> userManager, IMapper mapper,
            IRepository<AccGroupRole> groupRoleRepository,
            IRepository<AccGroup> groupRepository,
            IRepository<AccRole> roleRepository,
            IRepository<ApplicationUser> userRepository)
        {
            this._UserRepository = userRepository;
            this._roleManager = roleManager;
            this._UserManager = userManager;
            this._Mapper = mapper;
            this._GroupRoleRepository = groupRoleRepository;
            this._RoleRepository = roleRepository;
        }

        public async Task<ApiResult<RoleListDto>> CreateRole(RoleCuDto roleCuDto, CancellationToken cancellationToken)
        {
            var exists = await _roleManager.FindByNameAsync(roleCuDto.Name);
            if (exists != null)
                return new ApiResult<RoleListDto>(false, ApiResultStatusCode.NameIsExists, null);
            var entity = roleCuDto.ToEntity(_Mapper);

            var result = await _roleManager.CreateAsync(entity);

            if (result.Succeeded)
            {
                var role = _RoleRepository.TableNoTracking
                    .Where(x => x.Name == roleCuDto.Name).ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider).FirstOrDefault();
                return role;
            }
            return new ApiResult<RoleListDto>(false, ApiResultStatusCode.InsertFailed, null);

        }

        //public async Task<ApiResult<IPagedList<RoleListDto>>> Get(RoleSearchDto searchDto, CancellationToken cancellationToken)
        //{
        //    var expression = searchDto.GenerateExpression(searchDto);
        //    if (searchDto.RecordsPerPage > 50)
        //        return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded, null);

        //    var user = _UserRepository.GetById(searchDto.UserId);
        //    var userroles = await _UserManager.GetRolesAsync(user);

        //    var result = await _RoleRepository.TableNoTracking
        //        .Where(expression)
        //        .ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider)
        //        .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

        //    var resultList = new List<RoleListDto>();
        //    for (var i = 0; i < userroles.Count; i++)
        //    {
        //        var itemToRemove = result.Where(c => c.Name.Contains(userroles[i]));
        //        resultList = result.Except(itemToRemove).ToList();
        //    }

        //    if (result.Count == 0)
        //        return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.NotFound, null);

        //    var resultFinal = await resultList.ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

        //    return new ApiResult<IPagedList<RoleListDto>>(true, ApiResultStatusCode.Success, resultFinal, null,
        //        result.TotalItemCount,
        //        result.PageNumber, result.PageCount);
        //}

        public async Task<ApiResult<IPagedList<RoleListDto>>> Get(RoleSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);
            var result = await _RoleRepository.TableNoTracking
                .Where(expression).ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<RoleListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<IList<RoleListDto>>> GetAll(RoleSearchDto searchDto, CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);
            var result = await _RoleRepository.TableNoTracking
                .Where(expression).ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IList<RoleListDto>>(false, ApiResultStatusCode.NotFound, null, ApiResultStatusCode.NotFound.ToDisplay());

            return new ApiResult<IList<RoleListDto>>(true, ApiResultStatusCode.Success, result, ApiResultStatusCode.Success.ToDisplay());
        }

        public async Task<ApiResult<IPagedList<RoleListDto>>> GetRolesofGroupById(RoleSearchDto dto, CancellationToken cancellationToken)
        {
            if (dto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var roles = from groupRole in _GroupRoleRepository.TableNoTracking
                         join role in _RoleRepository.TableNoTracking on groupRole.RoleId equals role.Id
                         where groupRole.GroupId == dto.GroupId
                         select role;

            var expression = dto.GenerateExpression(dto);

            var result =
                await roles.
                Where(expression).
                ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider)
                .ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);

            if (result is { Count: 0 })
                return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<RoleListDto>>(true, ApiResultStatusCode.Success, result, null,
            result.TotalItemCount,
            result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<IPagedList<RoleListDto>>> GetRolesofUserById(RoleSearchDto dto, CancellationToken cancellationToken)
        {
            var user= await _UserManager.FindByIdAsync(dto.UserId.ToString());
            var roleNames =await _UserManager.GetRolesAsync(user);

            if (roleNames is null)
                return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.NotFound, null);

            var roles = _roleManager.Roles.Where(t1 => roleNames.Contains(t1.Name));

           var expression= dto.GenerateExpression(dto);

            var result = await roles
                  .Where(expression)
                  .ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider)
                .ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);


            if (result is { Count: 0 })
                return new ApiResult<IPagedList<RoleListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<RoleListDto>>(true, ApiResultStatusCode.Success, result, null,
            result.TotalItemCount,
            result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<RoleListDto>> RemoveRoleById(long id, CancellationToken cancellationToken)
        {
            var model = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(model);
            return new ApiResult<RoleListDto>(true, ApiResultStatusCode.Success, null);
        }

        public async Task<ApiResult<RoleListDto>> UpdateRole(RoleCuDto roleCuDto, CancellationToken cancellationToken)
        {
            var model = await _roleManager.FindByIdAsync(roleCuDto.Id.ToString());

            var result = await _roleManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                var role = _RoleRepository.TableNoTracking
                    .Where(x => x.Name == roleCuDto.Name).ProjectTo<RoleListDto>(_Mapper.ConfigurationProvider).FirstOrDefault();
                return role;
            }
            return new ApiResult<RoleListDto>(false, ApiResultStatusCode.UpdateFailed, null);
        }
    }
}
