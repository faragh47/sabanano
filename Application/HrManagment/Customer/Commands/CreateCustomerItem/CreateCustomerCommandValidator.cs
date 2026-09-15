using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly IRepository<Customer> _repository;

    public CreateCustomerCommandValidator(IRepository<Customer> repository)
    {
        _repository = repository;

        RuleFor(v => v.CustomerCode)
            .NotEmpty().WithMessage("کد مشتری اجباری است");

        RuleFor(x => x.CustomerCode)
            .Must(CustomerCode =>
              {
                 return !_repository.TableNoTracking.Any(x => x.CustomerCode == CustomerCode);
              })
                 .WithErrorCode(ApiResultStatusCode.CustomerCodeMustBeUnique.ToString())
                  .WithMessage(ApiResultStatusCode.CustomerCodeMustBeUnique.ToDisplay());

    }

}
