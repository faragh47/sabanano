
//using AutoMapper;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;


//using Common;
//using Common.Utilities;
//using Data.Contracts;
//using FluentValidation;


//public class UpdateFinancialDetailCommandValidator : AbstractValidator<UpdateFinancialDetailCommand>
//{
//    private readonly IRepository<FinancialDetail> _repository;
//    private readonly IMapper _mapper;

//    public UpdateFinancialDetailCommandValidator(IRepository<FinancialDetail> repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;

//        RuleFor(x => x.Id).NotNull()
//               .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
//               .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

//    }
//}
