
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateOrderCommandValidator()
    {
    }
}
