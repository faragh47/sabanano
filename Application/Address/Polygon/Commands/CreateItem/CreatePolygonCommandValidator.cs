using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreatePolygonCommandValidator : AbstractValidator<CreatePolygonCommand>
{
    private readonly IRepository<Polygon> _repository;

    public CreatePolygonCommandValidator(IRepository<Polygon> repository)
    {
        _repository = repository;

        //RuleFor(v => v.PostalCode)
        //    .NotEmpty().WithMessage("کدپستی اجباری است")
        //    .Length(10).WithMessage("فرمت کدپستی اشتباه است");

        //RuleFor(x => x.PostalCode)
        //    .Must(AddressId =>
        //      {
        //         return !_repository.TableNoTracking.Any(x => x.PostalCode == PostalCode);
        //      })
        //         .WithErrorCode(ApiResultStatusCode.PostalCodeMustBeUnique.ToString())
        //          .WithMessage(ApiResultStatusCode.PostalCodeMustBeUnique.ToDisplay());

    }

}
