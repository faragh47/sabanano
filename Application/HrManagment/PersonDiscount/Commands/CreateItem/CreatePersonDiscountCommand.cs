using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreatePersonDiscountCommand : BaseRecordDto<CreatePersonDiscountCommand, PersonDiscount, long>, IRequest<long>
{
    public long PersonId { get; set; }
    public long DiscountId { get; set; }
}

public class CreatePersonDiscountCommandHandler : IRequestHandler<CreatePersonDiscountCommand, long>
{
    private readonly IRepository<PersonDiscount> _repository;
    private readonly IMapper _mapper;

    public CreatePersonDiscountCommandHandler(IRepository<PersonDiscount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreatePersonDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new PersonDiscountCreatedEvent(entity));

        return entity.Id;
    }
}
