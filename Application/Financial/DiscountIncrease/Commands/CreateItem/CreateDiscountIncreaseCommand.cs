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

public record CreateDiscountIncreaseCommand : BaseRecordDto<CreateDiscountIncreaseCommand, DiscountIncrease, long>, IRequest<long>
{
    public long DistcountId { get; set; }
    public int Percent { get; set; }
}

public class CreateDiscountIncreaseCommandHandler : IRequestHandler<CreateDiscountIncreaseCommand, long>
{
    private readonly IRepository<DiscountIncrease> _repository;
    private readonly IMapper _mapper;

    public CreateDiscountIncreaseCommandHandler(IRepository<DiscountIncrease> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateDiscountIncreaseCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new DiscountIncreaseCreatedEvent(entity));

        return entity.Id;
    }
}
