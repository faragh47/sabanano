using CleanArchitecture.Domain.Entities;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    private readonly IRepository<Person> _repository;

    public CreatePersonCommandValidator(IRepository<Person> repository)
    {
        _repository = repository;



        RuleFor(x => x.MobileNumbers)
   .Must(mobilenumbers=>
   {

       return !(mobilenumbers is null || !mobilenumbers.Any());
   })
     .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
     .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());


        RuleFor(x => x.NationalId)
     .Must(nationalId =>
     {
         var exist = _repository.TableNoTracking.FirstOrDefault(x => x.NationalId == nationalId);
         var isNotNull = string.IsNullOrEmpty(exist?.NationalId);
         bool result = false;

         if (exist is not null)
             result = (exist is not null && isNotNull is false);

         return !result;
     })
     .WithErrorCode(ApiResultStatusCode.NationalIdExists.ToString())
     .WithMessage(ApiResultStatusCode.NationalIdExists.ToDisplay());

        RuleFor(x => x.MobileNumbers)
  .Must(mobilenumbers =>
  {
      return !_repository.TableNoTracking.Any(x => x.MobileNumbers.Any(x => mobilenumbers.Select(x => x.MobileNumber).Contains(x.MobileNumber)));
  })
  .WithErrorCode(ApiResultStatusCode.MobileExists.ToString())
  .WithMessage(ApiResultStatusCode.MobileExists.ToDisplay());

    }

}
