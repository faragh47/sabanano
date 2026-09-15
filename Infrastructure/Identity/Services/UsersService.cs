using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.SharedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.IServices.V2;
using X.PagedList;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.CustomExpressions;
using Microsoft.Extensions.Caching.Distributed;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

namespace Services.Services.V2
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AccRole> _roleManager;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<AccRole> _roleRepository;
        private readonly IRepository<AccGroupUser> _GroupUserRepository;
        private readonly IMapper _mapper;

        public UsersService(IRepository<ApplicationUser> userRepository, ILogger<UsersService> logger, IJwtService jwtService,
            UserManager<ApplicationUser> userManager, RoleManager<AccRole> roleManager, SignInManager<ApplicationUser> signInManager,
            IRepository<AccRole> roleRepository,
            IRepository<Person> personRepository,
            IRepository<AccGroupUser> groupUserRepository
            , IMapper mapper)
        {
            _logger = logger;
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
            _personRepository = personRepository;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _GroupUserRepository = groupUserRepository;
            _mapper = mapper;
        }
        //public async Task<List<ApplicationUser>> GetAllUsers(CancellationToken cancellationToken)
        //{
        //    return await _userRepository.TableNoTracking.ToListAsync<ApplicationUser>(cancellationToken);
        //}
        //public virtual async Task<ApiResult<IPagedList<UserListDto>>> Search(UserSearchDto searchDto,
        //    CancellationToken cancellationToken)
        //{
        //    var expression = searchDto.GenerateExpression(searchDto);

        //    if (searchDto.RecordsPerPage > 50)
        //        return new ApiResult<IPagedList<UserListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
        //            null);

        //    var result = await _userRepository.TableNoTracking
        //        .Where(expression).ProjectTo<UserListDto>(_mapper.ConfigurationProvider)
        //        .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 15, cancellationToken);
        //    if (result.Count == 0)
        //        return new ApiResult<IPagedList<UserListDto>>(false, ApiResultStatusCode.NotFound, null);

        //    return new ApiResult<IPagedList<UserListDto>>(true, ApiResultStatusCode.Success, result, null,
        //        result.TotalItemCount,
        //        result.PageNumber, result.PageCount);
        //}

        //public async Task<AccRole> CreateRole(AccRole role, CancellationToken cancellationToken)
        //{
        //    _logger.LogError("متد Create فراخوانی شد");

        //    var exists = await _roleManager.FindByNameAsync(role.Name);
        //    if (exists != null)
        //        throw new BadRequestException(ApiResultStatusCode.NameIsExists);

        //    var result = await _roleManager.CreateAsync(role);
        //    if (result.Succeeded)
        //        return role;

        //    throw new BadRequestException(result.Errors);
        //}

        public async Task<ApiResult<ApplicationUserListDto>> CreateUser(ApplicationUserCuDto user, long UserId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user.Password))
                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.BadRequest, null);

            var person = await _personRepository.TableNoTracking.FirstOrDefaultAsync(x=>x.Id==user.PersonId);

            if (person is null)
                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.BadRequest, null);

            var entity = user.ToEntity(_mapper);
            //var entity = new ApplicationUser();//user.ToEntity(_mapper);
            entity.CreationDate = DateTime.Now;
            entity.LastModificationDate = DateTime.Now;
            entity.CreatorId = UserId;
            entity.ModifierId = UserId;
            entity.IsActive = true;
            entity.FullName = person.FirstName + " " + person.LastName;

            var exists = await _userManager.FindByNameAsync(user.UserName);
            if (exists != null)
                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.NameIsExists, null);

            // var result = await _userManager.CreateUserDbAndCache(entity, user.Password);
            var result = await _userManager.CreateAsync(entity, user.Password);
            if (!result.Succeeded)
                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.InsertFailed, null);
            else
                return await _userRepository.TableNoTracking.ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Id == entity.Id, cancellationToken: cancellationToken);
        }

        public async Task<AccessToken> Login(string userName, string password, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);

            if (existingUser == null || existingUser.IsActive == false)
                throw new BadRequestException(ApiResultStatusCode.UserNameOrPasswordIsWrong.ToDisplay());

            //var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            var result = await _signInManager.CheckPasswordSignInAsync(existingUser, password, false);

            if (!result.Succeeded)
                throw new BadRequestException(ApiResultStatusCode.UserNameOrPasswordIsWrong.ToDisplay());

            var roles = await _userManager.GetRolesAsync(existingUser);

            //  var cacheUser = AuthorizationCache.ActiveUsersRoles.Where(x => x.UserId == existingUser.Id).FirstOrDefault();


            //if (cacheUser is null)
            //{
            //    AuthorizationCache.ActiveUsersRoles.Add(new UsersRole
            //    {
            //        UserId = existingUser.Id,
            //        Roles = roles?.ToList()
            //    });
            //}
            //else
            //    cacheUser.Roles = roles?.ToList();


            #region redis

            var userListDto = await _userRepository.TableNoTracking.Where(x => x.Id == existingUser.Id)
                    .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
           
            #endregion

            var jwt = await _jwtService.GenerateAsync(existingUser);
            jwt.roles = roles;
            jwt.PersonId = existingUser.PersonId;
            var userIdentity = await _signInManager.CreateUserPrincipalAsync(existingUser);

            jwt.Identity = userIdentity;
            //Update last login

            existingUser.LastLoginDate = DateTime.Now;
            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            return jwt;
        }

        //public async Task<ApplicationUser> AddRoleToUser(long userId, string roleName, CancellationToken cancellationToken)
        public async Task<ApiResult<ApplicationUserListDto>> AddRoleToUser(long userId, long roleId, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(cancellationToken, userId);
            if (existingUser == null)
                throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

            //var existingRole = _roleManager.FindByNameAsync(roleName);
            var existingRole = await _roleManager.FindByIdAsync(roleId.ToString());
            if (existingRole == null)
                throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

            var result = await _userManager.AddToRoleAsync(existingUser, existingRole.Name);
           // var result = await _identityService.AddToRoleDbAndCache(existingUser, existingRole.Name);

            if (!result.Succeeded)
                throw new BadRequestException(ApiResultStatusCode.ExistRoleInUser.ToDisplay());

            return _mapper.Map<ApplicationUserListDto>(existingUser);
            //return await _userRepository.TableNoTracking.ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Id == existingUser.Id, cancellationToken: cancellationToken);
        }

        public async Task<ApiResult<ApplicationUserListDto>> RemoveRoleFromUser(long userId, long roleId, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(cancellationToken, userId);
            if (existingUser == null)
                throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

            //var existingRole = _roleManager.FindByNameAsync(roleName);
            var existingRole = await _roleManager.FindByIdAsync(roleId.ToString());
            if (existingRole == null)
                throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

            return _mapper.Map<ApplicationUserListDto>(existingUser);
            //return await _userRepository.TableNoTracking.ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Id == existingUser.Id, cancellationToken: cancellationToken);
        }


        public async Task<ApiResult<List<ApplicationUserListDto>>> GetUsersByNationalId(ApplicationUserSearchDto user, CancellationToken cancellationToken)
        {
            var expresion = user.GenerateExpression(user);
            var result = await _userRepository.TableNoTracking.Where(expresion).ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (result is { Count: 0 })
                return new ApiResult<List<ApplicationUserListDto>>(false, ApiResultStatusCode.NotFound, null);

            return result;
        }

        
        public virtual async Task<ApiResult<IPagedList<ApplicationUserListDto>>> Get(ApplicationUserSearchDto searchDto,
            CancellationToken cancellationToken)
        {
            var expression = searchDto.GenerateExpression(searchDto);

            if (searchDto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);
            var result = await _userRepository.TableNoTracking
                .Where(expression).ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<ApplicationUserListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public virtual async Task<ApiResult<IPagedList<ApplicationUserListDto>>> GetAssignTicket(ApplicationUserSearchDto searchDto,
            CancellationToken cancellationToken)
        {
            var users = await _userManager.GetUsersInRoleAsync("Ticket-AssignToMe");
            var usersAssignToOthers = await _userManager.GetUsersInRoleAsync("Ticket-AssignToOthers");
            users.Concat(usersAssignToOthers);

            var usersId = await users.Select(x => x.Id).ToListAsync();

            var expression = searchDto.GenerateExpression(searchDto);

            expression = expression.AndExpression(x => usersId.Contains(x.Id));

            var result = await _userRepository.TableNoTracking
                .Where(expression).ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(searchDto.PageNumber ?? 1, searchDto.RecordsPerPage ?? 10, cancellationToken);

            if (result.Count == 0)
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<ApplicationUserListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public virtual async Task<ApiResult<ApplicationUserListDto>> ChangePassword(ApplicationUserSearchDto.ApplicationUserChangePasswordDto dto, bool isAdmin,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);


            if (user is null)
                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.UsernameIsNotValid, null);

            //var fullUser = await _userRepository.TableNoTracking.Include(x => x.Person).ThenInclude(x => x.IndividualPerson).FirstOrDefaultAsync(x => x.UserName == dto.UserName);
            //var nationalId = fullUser.Person.IndividualPerson.NationalId;

            var fullUser = new ApplicationUser();

            if (isAdmin)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
                if (result.Succeeded)
                {
                        await _userManager.UpdateAsync(user);
                    var userListDto = await _userRepository.TableNoTracking.Where(x => x.Id == user.Id)
                         .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                    return new ApiResult<ApplicationUserListDto>(true, ApiResultStatusCode.Success, null);
                }

                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.PasswordChangeFailed, null);
            }
            else
            {
                if (string.IsNullOrEmpty(dto.CurrentPassword))
                    return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.CurrentPasswordIsWrong, null);


                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (result.Succeeded)
                {
                        await _userManager.UpdateAsync(user);
                    var userListDto = await _userRepository.TableNoTracking.Where(x => x.Id == user.Id)
                         .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                    return new ApiResult<ApplicationUserListDto>(true, ApiResultStatusCode.Success, null);
                }

                return new ApiResult<ApplicationUserListDto>(false, ApiResultStatusCode.PasswordChangeFailed, null);
            }


        }

        public async Task<ApiResult<IPagedList<ApplicationUserListDto>>> GetUsersByRoleId(ApplicationUserSearchDto dto, CancellationToken cancellationToken)
        {
            if (dto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var role = await _roleManager.Roles.Where(r => r.Id == dto.RoleId).FirstOrDefaultAsync();

            if (role is null)
            {
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.NotFound, null);
            }
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            var expression = dto.GenerateExpression(dto);

            var result = await _userRepository.TableNoTracking
                .Where(t1 => users.Contains(t1))
                .Where(expression)
                .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);

            //  var usersFilter=await users.AsQueryable().Where(expression).ToListAsync();
            //  var result = await _mapper.Map<List<ApplicationUser>, List<ApplicationUserListDto>>(users.ToList())
            //  .ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);

            if (result is { Count: 0 })
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.NotFound, null);


            return new ApiResult<IPagedList<ApplicationUserListDto>>(true, ApiResultStatusCode.Success, result, null,
                result.TotalItemCount,
                result.PageNumber, result.PageCount);
        }

        public async Task<ApiResult<IPagedList<ApplicationUserListDto>>> GetUsersByGroupId(ApplicationUserSearchDto dto, CancellationToken cancellationToken)
        {
            if (dto.RecordsPerPage > 50)
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.MaximumRecordsPerPageExceeded,
                    null);

            var users = from groupUser in _GroupUserRepository.TableNoTracking
                        join user in _userRepository.TableNoTracking on groupUser.UserId equals user.Id
                        where groupUser.GroupId == dto.GroupId
                        select user;

            var expression = dto.GenerateExpression(dto);

            var result =
                await users.
                Where(expression)
                .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(dto.PageNumber ?? 1, dto.RecordsPerPage ?? 10, cancellationToken);

            if (result is { Count: 0 })
                return new ApiResult<IPagedList<ApplicationUserListDto>>(false, ApiResultStatusCode.NotFound, null);

            return new ApiResult<IPagedList<ApplicationUserListDto>>(true, ApiResultStatusCode.Success, result, null,
            result.TotalItemCount,
            result.PageNumber, result.PageCount);
        }

        public async Task<ApplicationUserListDto> UpdateUser(ApplicationUserCuDto user, long UserId, CancellationToken cancellationToken)
        {
            //var exists = await _userRepository.
            //    TableNoTracking.
            //    FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
            var exists = await _userManager.FindByIdAsync(user.Id.ToString());

            var entity = user.ToEntity(_mapper, exists);
            
            //var entity = new ApplicationUser();//user.ToEntity(_mapper);
     
            entity.LastModificationDate = DateTime.Now;
            entity.ModifierId = UserId;

            if (exists is null)
                throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

            var result = await _userManager.UpdateAsync(entity);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            var userListDto = await _userRepository.TableNoTracking.Where(x => x.Id == entity.Id)
                .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return await _userRepository.TableNoTracking
                .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync
                (x => x.Id == entity.Id, cancellationToken: cancellationToken);
        }
       
        public async Task<ApplicationUserListDto> GetUserById(long userId, CancellationToken cancellationToken)
        {
                var user = await _userManager.FindByIdAsync(userId.ToString());
               var userListDto = await _userRepository.TableNoTracking.Where(x => x.Id == userId)
                   .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return userListDto;
        }

        public async Task<ApiResult> UserIsInRole(long userId, string roleName, CancellationToken cancellationToken)
        {
            #region Redis
            var user = await _userRepository.TableNoTracking.Where(x => x.Id == userId).FirstOrDefaultAsync();
            #endregion
            //var user = await _userRepository.GetByIdAsync(cancellationToken, userId);
           var result = await _userManager.IsInRoleAsync(user, roleName);

            if(result is false)
                new ApiResult(result, ApiResultStatusCode.UnAuthorized, null);

            return
                new ApiResult(result, ApiResultStatusCode.Success, null);
        }

    

        public async Task<ApiResult<ApplicationUserListDto>> SetActiveUser(ApplicationUserActiveDto dto, long UserId, CancellationToken cancellationToken)
        {
            var exists = await _userManager.FindByIdAsync(dto.UserId.ToString());

            var entity = new ApplicationUser();//user.ToEntity(_mapper);
           // var entity = dto.ToEntity(_mapper, exists);
            entity.LastModificationDate = DateTime.Now;
            entity.ModifierId = UserId;

            if (exists is null)
                throw new BadRequestException(ApiResultStatusCode.NotFound.ToDisplay());

            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            var userListDto = await _userRepository.TableNoTracking.Where(x => x.Id == entity.Id)
                .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return await _userRepository.TableNoTracking
                .ProjectTo<ApplicationUserListDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken: cancellationToken);
        }

        public async Task<ApiResult<PeopleBriefDto>> GetPeopleByToken(long UserId, CancellationToken cancellationToken)
        {
            var user =await _userRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == UserId);
            var people =await _personRepository.TableNoTracking
                    .ProjectTo<PeopleBriefDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x=>x.Id== user.PersonId);

            return people;
        }
    }
}
