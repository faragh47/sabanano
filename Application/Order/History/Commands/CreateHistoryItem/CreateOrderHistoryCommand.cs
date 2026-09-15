using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using CleanArchitecture.Domain.Entities.Order;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.HrManagment;

public record CreateOrderHistoryCommand : BaseRecordDto<CreateOrderHistoryCommand, OrderHistory, long>, IRequest<long>
{
    public string TechnicalComment { get; set; }
    public long OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}

public class CreateOrderHistoryCommandHandler : IRequestHandler<CreateOrderHistoryCommand, long>
{
    private readonly IRepository<OrderHistory> _repository;
    private readonly IMapper _mapper;

    public CreateOrderHistoryCommandHandler(IRepository<OrderHistory> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateOrderHistoryCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}