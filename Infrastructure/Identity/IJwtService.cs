using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Identity;
using Common;

namespace Services
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(ApplicationUser user);
        int? ValidateToken(string token);
    }
}