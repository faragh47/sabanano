using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateOrder;

public record SendOrderToFinancialCommand : BaseRecordDto<SendOrderToFinancialCommand, Order, long>,
    IRequest<long>
{
    public long ImageId { get; set; }

    public override void CustomMappings(IMappingExpression<SendOrderToFinancialCommand, Order> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class SendOrderToFinancialCommandHandler : IRequestHandler<SendOrderToFinancialCommand, long>
{
    private readonly IRepository<Order> _repository;
    private readonly IMapper _mapper;

    public SendOrderToFinancialCommandHandler(IRepository<Order> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(SendOrderToFinancialCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.TableNoTracking
            .Include(x=>x.Financial)
            .OrderByDescending(x => x.Created)
            // .Where(x => x.OrderStatus.Id == OrderStatus.Initial.Id)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        order.OrderStatus = OrderStatus.WaitingForFinancial;
        order.Financial.Payment=new Payment()
        {
            ImageId =request.ImageId,
            TypeId = PaymentType.PaymentWithAtthachment.Id
        };
        await _repository.UpdateAsync(order, cancellationToken);
        return order.Id;
    }
}