using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;
using CleanArchitecture.Domain.ValueObjects; 
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    private readonly IRepository<Account> _AccountRepository;

    public CreateAccountCommandValidator( IRepository<Account> AccountRepository)
    {
        _AccountRepository = AccountRepository;

       // RuleFor(x => x.AccountTypeId).NotNull()
       //      .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //      .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

       // RuleFor(x => x.ProviderId).NotNull()
       //      .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //      .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

       // RuleFor(x => x.Price).NotNull()
       //           .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //           .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

       // RuleFor(x => x.Title).NotNull()
       //       .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //       .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

       // RuleFor(x => x.AccountId).NotNull()
       //.WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //.WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());


       // RuleFor(x => new { x.AccountId })
       //     .Must(input =>
       //     {
       //        var Account= AccountRepository.TableNoTracking.FirstOrDefault(x=>x.Id==input.AccountId);

       //         return Account.ServiceTypeId == ServiceType.Reservation.Id;
       //     })
       //          .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //          .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());




    }

}
