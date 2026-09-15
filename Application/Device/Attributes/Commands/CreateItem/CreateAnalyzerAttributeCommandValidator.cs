using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;

public class CreateAnalyzeDeviceAttributeCommandValidator : AbstractValidator<CreateAnalyzeDeviceAttributeCommand>
{
    private readonly IRepository<AnalyzeDeviceAttribute> _repository;

    public CreateAnalyzeDeviceAttributeCommandValidator(IRepository<AnalyzeDeviceAttribute> repository)
    {
        _repository = repository;

        RuleFor(x => x.AnalyerDeviceId).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

        RuleFor(x => x.SampleCategoryId).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());
    }

}
