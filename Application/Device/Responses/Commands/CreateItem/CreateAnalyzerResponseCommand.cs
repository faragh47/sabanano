using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateAnalyzeResponseCommand : BaseRecordDto<CreateAnalyzeResponseCommand, AnalyzeDeviceResponse, int>, IRequest<int>
{
    public int AnalyzeDeviceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ImageCuDto Image { get; set; }
}

public class CreateAnalyzeResponseCommandHandler : IRequestHandler<CreateAnalyzeResponseCommand, int>
{
    private readonly IRepository<AnalyzeDeviceResponse> _repository;
    private readonly IMapper _mapper;
    private ISender _mediator;
    public CreateAnalyzeResponseCommandHandler(IRepository<AnalyzeDeviceResponse> repository,
        IMapper mapper, ISender mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateAnalyzeResponseCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        try
        {
            if (request.Image is not null)
            {
                CreateImageCommand command = new CreateImageCommand()
                {
                    ImageFile = request.Image.ImageFile,
                    FolderPath = request.Image.FolderPath,
                    ImageFileNameWithExt = request.Image.ImageFileNameWithExt,
                    ImageSizeInBytes = request.Image.ImageSizeInBytes,
                    ImageTitle = request.Image.ImageTitle,
                    ImageLatinTitle = request.Image.ImageLatinTitle,
                    InternalUsage = request.Image.InternalUsage,
                };

                var requestImage = await _mediator.Send(command);
                entity.ImageId = requestImage;
            }

            await _repository.AddAsync(entity, cancellationToken);
            return entity.Id;

        }
        catch (Exception ex)
        {

            //TODO: Throw appropriate error
            var errormessage = ex.Message;
            throw;
        }

        // entity.AddDomainEvent(new PeopleCreatedEvent(entity));

    }
}
