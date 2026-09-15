using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    private readonly IRepository<Address> _repository;

    public CreateAddressCommandValidator(IRepository<Address> repository)
    {
        _repository = repository;

        RuleFor(v => v.PostalCode)
            .NotEmpty().WithMessage("کدپستی اجباری است")
            .Length(10).WithMessage("فرمت کدپستی اشتباه است");

        RuleFor(x => x.PostalCode)
            .Must(PostalCode =>
              {
                 return !_repository.TableNoTracking.Any(x => x.PostalCode == PostalCode);
              })
                 .WithErrorCode(ApiResultStatusCode.PostalCodeMustBeUnique.ToString())
                  .WithMessage(ApiResultStatusCode.PostalCodeMustBeUnique.ToDisplay());

    }

}
