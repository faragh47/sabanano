using Common;
using Common.Utilities;
using FluentValidation;

namespace CleanArchitecture.Application.Financial.Discount.Commands.UpdateItem;

public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
{

    public UpdateDiscountCommandValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

        RuleFor(x => x.Percent)
            .Must(Percent =>
            {
                if (Percent is not null)
                {
                    return Percent >= 0 && Percent <= 100;
                }
                else
                {
                    return true;
                }
            })
                 .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
                 .WithMessage("درصد باید بین 1 تا 100 باشد.");
    }
}
