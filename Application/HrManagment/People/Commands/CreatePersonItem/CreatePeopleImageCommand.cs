using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreatePeopleImageCommand : BaseRecordDto<CreatePeopleImageCommand, Person, long>, IRequest<long>
{
    public long PeopleId { get; set; }
    public long ImageId { get; set; }
    public bool InternalUsage { get; set; }
    public bool IsApprove { get; set; }
    public long userId { get; set; }
    public string ImageTitle { get; set; }
    public string ImageLatinTitle { get; set; }
    public string ImageFileNameWithExt { get; set; }
    public long ImageSizeInBytes { get; set; }
    public IFormFile ImageFile { get; set; }
    public string FolderPath { get; set; }
}

public class CreatePeopleImageCommandHandler : IRequestHandler<CreatePeopleImageCommand, long>
{
    private readonly IRepository<Person> _repository;
    private readonly IMapper _mapper;
    private ISender _mediator;
    public CreatePeopleImageCommandHandler(IRepository<Person> repository,
        IMapper mapper, ISender mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<long> Handle(CreatePeopleImageCommand request, CancellationToken cancellationToken)
    {

        CreateImageCommand command = new CreateImageCommand()
        {
            ImageFile = request.ImageFile,
            FolderPath = request.FolderPath,
            ImageFileNameWithExt = request.ImageFileNameWithExt,
            ImageSizeInBytes = request.ImageSizeInBytes,
            ImageTitle = request.ImageTitle,
            ImageLatinTitle = request.ImageLatinTitle,
            InternalUsage = request.InternalUsage,
            ContextId = request.PeopleId,
        };

        var requestImage = await _mediator.Send(command);

        try
        {
            var entity = await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.PeopleId);
            entity.ImageId = requestImage;
            await _repository.UpdateAsync(entity, cancellationToken);
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
