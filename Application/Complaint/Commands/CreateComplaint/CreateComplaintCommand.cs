using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public record CreateComplaintCommand : BaseRecordDto<CreateComplaintCommand, Complaint, int>, IRequest<int>
{
    public string? TrackingCodes { get; set; }
    public string? OrderDescription { get; set; }
    public long? ImageId { get; set; }
    public string Description { get; set; }
    public string Recomendation { get; set; }
}

public class CreateComplaintCommandCommandHandler : IRequestHandler<CreateComplaintCommand, int>
{
    private readonly IRepository<Complaint> _repository;
    private readonly IMapper _mapper;

    public CreateComplaintCommandCommandHandler(IRepository<Complaint> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}