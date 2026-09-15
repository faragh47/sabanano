//using CleanArchitecture.Domain.Entities;
//using CleanArchitecture.Domain.Entities.HrManagment;
//using Common;
//using Common.Utilities;
//using Data.Contracts;
//using FluentValidation;

//using CleanArchitecture.Domain.ValueObjects; 

//using CleanArchitecture.Domain.Entities.FinancialAggregate;


//public class CreateFinancialCommandValidator : AbstractValidator<CreateFinancialCommand>
//{
//    private readonly IRepository<Financial> _repository;
//    //private readonly IRepository<Order> _orderRepository;

//    public CreateFinancialCommandValidator(IRepository<Financial> repository, IRepository<Order> orderRepository)
//    {
//        _repository = repository;
//        _orderRepository = orderRepository;

//       // RuleFor(x => x.FinancialTypeId).NotNull()
//       //      .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//       //      .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//       // RuleFor(x => x.ProviderId).NotNull()
//       //      .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//       //      .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//       // RuleFor(x => x.Price).NotNull()
//       //           .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//       //           .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//       // RuleFor(x => x.Title).NotNull()
//       //       .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//       //       .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//       // RuleFor(x => x.OrderId).NotNull()
//       //.WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//       //.WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());


//       // RuleFor(x => new { x.OrderId })
//       //     .Must(input =>
//       //     {
//       //        var order= orderRepository.TableNoTracking.FirstOrDefault(x=>x.Id==input.OrderId);

//       //         return order.ServiceTypeId == ServiceType.Reservation.Id;
//       //     })
//       //          .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//       //          .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());




//    }

//}
