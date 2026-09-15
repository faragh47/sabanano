
//using AutoMapper;
//using CleanArchitecture.Application.People.Commands.UpdateAccount;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;
//using Common;
//using Common.Utilities;
//using Data.Contracts;
//using FluentValidation;

//namespace CleanArchitecture.Application.People.Commands.UpdateAccountType;

//public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
//{
//    private readonly IRepository<Account> _repository;
//    private readonly IMapper _mapper;

//    public UpdateAccountCommandValidator(IRepository<Account> repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;

//        RuleFor(x => x.Id).NotNull()
//               .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//               .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//    }
//}
