
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class SendOrderToTechnicalExpertCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public SendOrderToTechnicalExpertCommandValidator()
    {
    }
}
