using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateOrder;

public record ConfirmOfGetAnalyzeCommand : BaseRecordDto<ConfirmOfGetAnalyzeCommand, Order, long>,
    IRequest<long>
{
    public string TechnicalComment { get; set; }

    public override void CustomMappings(IMappingExpression<ConfirmOfGetAnalyzeCommand, Order> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class ConfirmOfGetAnalyzeCommandHandler : IRequestHandler<ConfirmOfGetAnalyzeCommand, long>
{
    private readonly IRepository<Order> _repository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ConfirmOfGetAnalyzeCommandHandler(IRepository<Order> repository,
        IMapper mapper,
        IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<long> Handle(ConfirmOfGetAnalyzeCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.TableNoTracking
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync(x => x.OrderStatus.Id == OrderStatus.FinancialConfirmed.Id
                                      && x.Id == request.Id);
        await _mediator.Send(new CreateOrderHistoryCommand()
        {
            OrderStatus = OrderStatus.Proccessing,
            OrderId = request.Id,
            TechnicalComment = request.TechnicalComment
        });
        order.OrderStatus = OrderStatus.Proccessing;
        await _repository.UpdateAsync(order, cancellationToken);
        return order.Id;
    }
}