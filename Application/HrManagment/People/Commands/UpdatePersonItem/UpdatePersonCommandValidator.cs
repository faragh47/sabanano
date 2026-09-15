
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdatePerson;

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
    }
}
