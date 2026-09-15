using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects; 

using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateFinancialDetailCommandValidator : AbstractValidator<CreateFinancialDetailCommand>
{
    private readonly IRepository<FinancialDetail> _repository;

    public CreateFinancialDetailCommandValidator(IRepository<FinancialDetail> repository)
    {
        _repository = repository;

       // RuleFor(x => x.FinancialDetailTypeId).NotNull()
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

       // RuleFor(x => x.OrderId).NotNull()
       //.WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //.WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());


       // RuleFor(x => new { x.OrderId })
       //     .Must(input =>
       //     {
       //        var order= orderRepository.TableNoTracking.FirstOrDefault(x=>x.Id==input.OrderId);

       //         return order.ServiceTypeId == ServiceType.Reservation.Id;
       //     })
       //          .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
       //          .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());




    }

}
