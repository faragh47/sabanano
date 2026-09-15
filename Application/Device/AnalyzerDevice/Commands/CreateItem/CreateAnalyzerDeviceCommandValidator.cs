using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;

public class CreateAnalyzerDeviceCommandValidator : AbstractValidator<CreateAnalyzerDeviceCommand>
{
    private readonly IRepository<AnalyzerDevice> _repository;

    public CreateAnalyzerDeviceCommandValidator(IRepository<AnalyzerDevice> repository)
    {
        _repository = repository;

        RuleFor(x => x.Name).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

    }

}
