using System;
using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;

namespace WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId =>
        Convert.ToInt64(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));

    public string UserName => Convert.ToString(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));

    public string FullName =>
        Convert.ToString(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Actor));

    public bool IsInRole(string roleName) => _httpContextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;
    public string Email => Convert.ToString(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email));
}