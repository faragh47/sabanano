
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class ConfirmFinancialCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public ConfirmFinancialCommandValidator()
    {
    }
}
