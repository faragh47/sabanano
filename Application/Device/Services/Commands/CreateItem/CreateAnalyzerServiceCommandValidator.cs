using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;

public class CreateAnalyzeDeviceServiceCommandValidator : AbstractValidator<CreateAnalyzeDeviceServiceCommand>
{
    private readonly IRepository<AnalyzeDeviceService> _repository;

    public CreateAnalyzeDeviceServiceCommandValidator(IRepository<AnalyzeDeviceService> repository)
    {
        _repository = repository;

        RuleFor(x => x.AnalyzeDeviceId).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

    }

}
