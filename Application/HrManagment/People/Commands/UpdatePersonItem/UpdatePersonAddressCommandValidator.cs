
using Common;
using Common.Utilities;
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdatePeopleAddress;

public class UpdatePeopleAddressCommandValidator : AbstractValidator<UpdatePeopleAddressCommand>
{
    public UpdatePeopleAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotNull()
               .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
               .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

        RuleFor(x => x.PersonId).NotNull()
         .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
         .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

        RuleFor(x => x.Address).NotNull()
.WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
.WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

        //        RuleFor(x => x.Address.Id).NotNull()
        // .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
        // .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());
    }
}
