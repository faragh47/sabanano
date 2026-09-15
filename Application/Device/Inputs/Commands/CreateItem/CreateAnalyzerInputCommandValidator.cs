using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;

public class CreateAnalyzeDeviceInputCommandValidator : AbstractValidator<CreateAnalyzeDeviceInputCommand>
{
    private readonly IRepository<AnalyzeDeviceInput> _repository;

    public CreateAnalyzeDeviceInputCommandValidator(IRepository<AnalyzeDeviceInput> repository)
    {
        _repository = repository;

        RuleFor(x => x.AnalyzeDeviceId).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

    }

}
