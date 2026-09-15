using Common;
using Common.Exceptions;
using FluentValidation.Results;

namespace CleanArchitecture.Application.Common.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(string[] strings)
        : base(ApiResultStatusCode.BadRequest, strings)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this(failures.Select(x=>x.ErrorMessage).ToArray())
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
