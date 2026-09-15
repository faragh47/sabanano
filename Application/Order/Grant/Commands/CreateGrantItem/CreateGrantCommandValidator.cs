using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.HrManagment;

public class CreateGrantCommandValidator : AbstractValidator<CreateGrantCommand>
{
    public CreateGrantCommandValidator()
    {
    }
}
