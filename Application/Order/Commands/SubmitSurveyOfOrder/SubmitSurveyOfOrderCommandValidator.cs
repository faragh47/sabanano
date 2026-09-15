
using CleanArchitecture.Application.People.Commands.UpdateOrder;
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public class SubmitSurveyOfOrderCommandValidator : AbstractValidator<SubmitSurveyOfOrderCommand>
{
    public SubmitSurveyOfOrderCommandValidator()
    {
    }
}
