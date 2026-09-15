using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IRepository<ApplicationUser> userRepository)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _userRepository = userRepository;
    }

    public async Task<string> GetUserNameAsync(long userId)
    {
        var user = await _userRepository.TableNoTracking.FirstOrDefaultAsync(u => u.Id == userId);

        return user?.UserName;
    }

    public string GetUserName(long userId)
    {
        var user = _userRepository.TableNoTracking.FirstOrDefault(u => u.Id == userId);

        return user?.UserName;
    }

    public async Task<string> GetFullNameAsync(long userId)
    {
        var user = await _userRepository.TableNoTracking.Include(x => x.Person)
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user?.Person?.FirstName + " " + user?.Person?.LastName;
    }

    public string GetFullName(long userId)
    {
        var user = _userRepository.TableNoTracking.Include(x => x.Person).FirstOrDefault(u => u.Id == userId);
        return user?.Person?.FirstName + " " + user?.Person?.LastName;
    }

    public async Task<long?> GetPersonId(long userId)
    {
        var user = await _userRepository.TableNoTracking
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user?.PersonId;
    }

    public ApplicationUserDto GetUser(long userId)
    {
        var user = _userRepository.TableNoTracking
            .Include(x => x.Person)
            .ThenInclude(x => x.PeopleAddresses)
            .Include(x => x.Person)
            .ThenInclude(x => x.MobileNumbers)
            .FirstOrDefault(u => u.Id == userId);
        return new ApplicationUserDto()
        {
            FullName = user?.FullName ?? "",
            NationalCode = user?.Person?.NationalId ?? "",
            MobileNumber = user?.Person?.MobileNumbers?.FirstOrDefault()?.MobileNumber ?? "",
            Email = user?.Email ?? "",
            Address = user?.Person?.PeopleAddresses?.FirstOrDefault()?.Address?.FullAddress ?? ""
        };
    }

    public string GetEmail(long userId)
    {
        var user = _userRepository.TableNoTracking.FirstOrDefault(u => u.Id == userId);
        return user?.Email;
    }

    public async Task<(Result Result, long UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(long userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(long userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(long userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}