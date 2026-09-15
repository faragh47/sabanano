
using AutoMapper;
using CleanArchitecture.Domain.Entities.Device;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;


public class UpdateAnalyzerDeviceCommandValidator : AbstractValidator<UpdateAnalyzerDeviceCommand>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IMapper _mapper;

    public UpdateAnalyzerDeviceCommandValidator(IRepository<AnalyzerDevice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

        RuleFor(x => x.Id).NotNull()
               .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
               .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

    }
}
