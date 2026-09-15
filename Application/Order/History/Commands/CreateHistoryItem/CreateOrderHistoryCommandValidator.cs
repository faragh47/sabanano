using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.HrManagment;

public class CreateOrderHistoryListDtoCommandValidator : AbstractValidator<CreateOrderHistoryCommand>
{
    public CreateOrderHistoryListDtoCommandValidator()
    {
    }
}
