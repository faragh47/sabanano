
using CleanArchitecture.Application.People.Commands.UpdatePersonDiscount;
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateePersonDiscountType;

public class UpdatePersonDiscountCommandValidator : AbstractValidator<UpdatePersonDiscountCommand>
{
    public UpdatePersonDiscountCommandValidator()
    {
    }
}
