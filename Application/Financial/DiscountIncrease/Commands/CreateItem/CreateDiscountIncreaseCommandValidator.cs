using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateDiscountIncreaseCommandValidator : AbstractValidator<CreateDiscountIncreaseCommand>
{
    private readonly IRepository<DiscountIncrease> _repository;

    public CreateDiscountIncreaseCommandValidator(IRepository<DiscountIncrease> repository)
    {
        _repository = repository;

    }

}
