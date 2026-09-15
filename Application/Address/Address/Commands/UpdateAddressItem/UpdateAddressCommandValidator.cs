
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
    }
}
