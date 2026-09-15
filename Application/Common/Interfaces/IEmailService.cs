using CleanArchitecture.Application.Common;
using Common;
using DataTransferObjects.SharedModels;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IEmailService:IScopedDependency
{
    Task<ApiResult> SendEmail(Email email);
}
