
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class SetFinancialOfOrderCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public SetFinancialOfOrderCommandValidator()
    {
    }
}
