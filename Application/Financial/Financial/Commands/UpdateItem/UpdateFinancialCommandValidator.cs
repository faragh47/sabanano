
//using AutoMapper;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;


//using Common;
//using Common.Utilities;
//using Data.Contracts;
//using FluentValidation;


//public class UpdateFinancialCommandValidator : AbstractValidator<UpdateFinancialCommand>
//{
//    private readonly IRepository<Financial> _repository;
//    private readonly IMapper _mapper;

//    public UpdateFinancialCommandValidator(IRepository<Financial> repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;

//        RuleFor(x => x.Id).NotNull()
//               .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//               .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//    }
//}
