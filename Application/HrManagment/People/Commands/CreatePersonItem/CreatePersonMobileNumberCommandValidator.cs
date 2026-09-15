using CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
using CleanArchitecture.Domain.Entities;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreatePersonMobileNumberCommandValidator : AbstractValidator<CreatePersonMobileNumberCommand>
{
    private readonly IRepository<PersonMobileNumber> _repository;

    public CreatePersonMobileNumberCommandValidator(IRepository<PersonMobileNumber> repository)
    {
        _repository = repository;

        RuleFor(x => x.MobileNumber)
     .Must(MobileNumber =>
     {
         return !_repository.TableNoTracking.Any(x => x.MobileNumber == MobileNumber);
     })
     .WithErrorCode(ApiResultStatusCode.MobileExists.ToString())
     .WithMessage(ApiResultStatusCode.MobileExists.ToDisplay());

        RuleFor(x => x.MobileNumber)
        .Length(11)
        .WithMessage("شماره موبایل باید 11 رقم باشد.");


        RuleFor(x => x.MobileNumber).Must(MobileNumber =>
        {
            var IsDigit = MobileNumber.IsDigitsOnly();

            return IsDigit;
        })
        .WithErrorCode(ApiResultStatusCode.MobileExists.ToString())
        .WithMessage("شماره موبایل باید عدد باشد");

        RuleFor(x => x.MobileNumber).Must(MobileNumber =>
        {
            var IsDigit = MobileNumber.StartsWith("0");

            return IsDigit;
        })
        .WithErrorCode(ApiResultStatusCode.MobileExists.ToString())
        .WithMessage("شماره موبایل باید با 0 شروع شود");

        RuleFor(v => v.MobileNumber)
        .NotEmpty().WithMessage("شماره موبایل اجباری است");
    }

}
