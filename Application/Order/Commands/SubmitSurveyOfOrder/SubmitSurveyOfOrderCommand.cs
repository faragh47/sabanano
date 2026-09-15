using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.People.Commands.UpdateOrder;

public record SubmitSurveyOfOrderCommand : BaseRecordDto<SubmitSurveyOfOrderCommand, OrderSurvey, long>,
    IRequest<long>
{
    public string Comment { get; set; }
    public SurveyScore Score { get; set; }
    public long OrderId { get; set; }

    public override void CustomMappings(IMappingExpression<SubmitSurveyOfOrderCommand, OrderSurvey> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class SubmitSurveyOfOrderCommandHandler : IRequestHandler<SubmitSurveyOfOrderCommand, long>
{
    private readonly IRepository<OrderSurvey> _repository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SubmitSurveyOfOrderCommandHandler(IRepository<OrderSurvey> repository,
        IMapper mapper,
        IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<long> Handle(SubmitSurveyOfOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}