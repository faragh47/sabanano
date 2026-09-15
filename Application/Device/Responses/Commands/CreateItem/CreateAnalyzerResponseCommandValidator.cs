using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateAnalyzeResponseCommandValidator : AbstractValidator<CreateAnalyzeResponseCommand>
{
    private readonly IRepository<AnalyzeDeviceResponse> _repository;

    public CreateAnalyzeResponseCommandValidator(IRepository<AnalyzeDeviceResponse> repository)
    {
        _repository = repository;

        RuleFor(x => x.AnalyzeDeviceId).NotNull()
              .WithErrorCode(ApiResultStatusCode.BadRequest.ToString())
              .WithMessage(ApiResultStatusCode.BadRequest.ToDisplay());

    }

}
