using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(long userId);
    Task<string> GetFullNameAsync(long userId);
    string GetUserName(long userId);
    string GetFullName(long userId);
    Task<long?> GetPersonId(long userId);
    ApplicationUserDto GetUser(long userId);
    Task<bool> IsInRoleAsync(long userId, string role);
    Task<bool> AuthorizeAsync(long userId, string policyName);

    Task<(Result Result, long UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(long userId);
}