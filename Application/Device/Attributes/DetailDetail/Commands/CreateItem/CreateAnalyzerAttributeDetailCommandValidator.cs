using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;

public class CreateAnalyzeDeviceAttributeDetailCommandValidator : AbstractValidator<CreateAnalyzeDeviceAttributeDetailCommand>
{
    private readonly IRepository<AnalyzeDeviceAttributeDetail> _repository;

    public CreateAnalyzeDeviceAttributeDetailCommandValidator(IRepository<AnalyzeDeviceAttributeDetail> repository)
    {
        _repository = repository;
    }

}
