using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateDiscountUsedCommand : BaseRecordDto<CreateDiscountUsedCommand, DiscountUsed, long>, IRequest<long>
{
    public long DiscountId { get; set; }
    public long PersonId { get; set; }
}

public class CreateDiscountUsedCommandHandler : IRequestHandler<CreateDiscountUsedCommand, long>
{
    private readonly IRepository<DiscountUsed> _repository;
    private readonly IMapper _mapper;

    public CreateDiscountUsedCommandHandler(IRepository<DiscountUsed> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateDiscountUsedCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new DiscountUsedCreatedEvent(entity));

        return entity.Id;
    }
}
