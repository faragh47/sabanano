using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateOrder;

public record SetFinancialOfOrderCommand : BaseRecordDto<SetFinancialOfOrderCommand, Order, long>,
    IRequest<long>
{
    public decimal TotalPayablePrice  { get; set; }
    public decimal GrantPrice { get; set; }
    public decimal AdditionalPrice { get; set; }
    public decimal AnalyzePrice { get; set; }
    public decimal Tax { get; set; }
    public override void CustomMappings(IMappingExpression<SetFinancialOfOrderCommand, Order> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class SetFinancialOfOrderCommandHandler : IRequestHandler<SetFinancialOfOrderCommand, long>
{
    private readonly IRepository<Order> _repository;
    private readonly IMapper _mapper;

    public SetFinancialOfOrderCommandHandler(IRepository<Order> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(SetFinancialOfOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.TableNoTracking
            .Include(x => x.Financial)
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (order.Financial?.StatusId == FinancialStatus.Payed.Id)
            throw new BadRequestException("Order is paid");

        order.Financial = new Domain.Entities.FinancialAggregate.Financial()
        {
            Title = "مبلغ قابل پرداخت",
            StatusId = FinancialStatus.WaitForPayment.Id,
            Tax = request.Tax,
            TotalPayablePrice= request.TotalPayablePrice,
            GrantPrice = request.GrantPrice,
            AdditionalPrice = request.AdditionalPrice,
            AnalyzePrice = request.AnalyzePrice,
        };
        order.OrderStatus=OrderStatus.SendToFinancial;
        await _repository.UpdateAsync(order, cancellationToken);
        return order.Id;
    }
}