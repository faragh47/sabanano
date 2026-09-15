
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class SendOrderToFinancialCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public SendOrderToFinancialCommandValidator()
    {
    }
}
