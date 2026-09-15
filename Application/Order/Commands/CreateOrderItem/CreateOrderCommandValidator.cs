using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace  CleanArchitecture.Application.Orders.Commands.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IRepository<Address> _repository;

    public CreateOrderCommandValidator(IRepository<Address> repository)
    {
        _repository = repository;
    }

}
