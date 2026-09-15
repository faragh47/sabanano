using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreatePersonDiscountCommandValidator : AbstractValidator<CreatePersonDiscountCommand>
{
    private readonly IRepository<PersonDiscount> _repository;

    public CreatePersonDiscountCommandValidator(IRepository<PersonDiscount> repository)
    {
        _repository = repository;

        RuleFor(x => new {x.DiscountId,x.PersonId })
            .Must(input =>
              {
                  return !_repository.TableNoTracking.Any(x => x.DiscountId == input.DiscountId && x.PersonId == input.PersonId);
              })
                 .WithErrorCode(ApiResultStatusCode.InformationExists.ToString())
                 .WithMessage(ApiResultStatusCode.InformationExists.ToDisplay());

    }

}
