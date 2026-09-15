
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class RejectTechnicalExpertCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public RejectTechnicalExpertCommandValidator()
    {
    }
}
