using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;


public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    private readonly IRepository<Discount> _repository;

    public CreateDiscountCommandValidator(IRepository<Discount> repository)
    {
        _repository = repository;

        RuleFor(x => x.Code)
            .Must(Code =>
              {
                  return !_repository.TableNoTracking.Any(x => x.Code == Code);
              })
                 .WithErrorCode(ApiResultStatusCode.InformationExists.ToString())
                 .WithMessage(ApiResultStatusCode.InformationExists.ToDisplay());


        RuleFor(x => x.Percent)
            .Must(Percent =>
            {
                if (Percent is not null)
                {
                    return Percent >= 0 && Percent <= 100;
                }
                else
                {
                    return true;
                }
            })
                 .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
                 .WithMessage("درصد باید بین 1 تا 100 باشد.");

    }

}
