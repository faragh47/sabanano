namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICurrentUserService
{
    long? UserId { get; }
    string UserName { get; }
    string FullName { get; }
    bool IsInRole(string roleName);
    string Email { get; }
}
