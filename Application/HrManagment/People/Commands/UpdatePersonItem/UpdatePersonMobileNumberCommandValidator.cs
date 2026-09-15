
using Common;
using Common.Utilities;
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdatePersonMobileNumber;

public class UpdatePersonMobileNumberCommandValidator : AbstractValidator<UpdatePersonMobileNumberCommand>
{
    public UpdatePersonMobileNumberCommandValidator()
    {
        RuleFor(x => x.Id).NotNull()
               .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
               .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());


    }
}
