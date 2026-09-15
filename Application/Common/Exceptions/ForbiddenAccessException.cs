using Common.Exceptions;

namespace CleanArchitecture.Application.Common.Exceptions;

public class ForbiddenAccessException : AppException
{
    public ForbiddenAccessException() : base() { }
}
