using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Email;

public class CreateEmailDiscountCommandValidator : AbstractValidator<CreateEmailDiscountCommand>
{
    private readonly IRepository<EmailDiscount> _repository;

    public CreateEmailDiscountCommandValidator(IRepository<EmailDiscount> repository)
    {
        _repository = repository;

        RuleFor(x => x.Email).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

    }

}
