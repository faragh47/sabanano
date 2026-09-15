using System;
using Common;
using DataTransferObjects.SharedModels;
using Services;

namespace CleanArchitecture.Infrastructure.Services
{
    public interface IUserCodeService : IScopedDependency
    {
        Task<ApiResult<AccessToken>> Token(CodeWithMobileNumber dto, CancellationToken cancellationToken);
        Task<ApiResult<RegisterDto>> GenerateCode(string MobileNumber, CancellationToken cancellationToken);
    }
}

