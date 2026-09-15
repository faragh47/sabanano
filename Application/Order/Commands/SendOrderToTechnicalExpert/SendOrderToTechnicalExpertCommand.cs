using System.CodeDom.Compiler;
using System.Globalization;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateOrder;

public record SendOrderToTechnicalExpertCommand : BaseRecordDto<SendOrderToTechnicalExpertCommand, Order, long>,
    IRequest<long>
{
    public override void CustomMappings(IMappingExpression<SendOrderToTechnicalExpertCommand, Order> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class SendOrderToTechnicalExpertCommandHandler : IRequestHandler<SendOrderToTechnicalExpertCommand, long>
{
    private readonly IRepository<Order> _repository;

    public SendOrderToTechnicalExpertCommandHandler(IRepository<Order> repository,
        IMapper mapper,
        IRepository<AnalyzerDevice> analyzeDeviceRepository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(SendOrderToTechnicalExpertCommand request, CancellationToken cancellationToken)
    {
        var orders = await _repository.TableNoTracking
            .Include(x => x.OrderAnalyzes)
            .OrderByDescending(x => x.Created)
            .Where(x => x.OrderStatus.Id == OrderStatus.Initial.Id)
            .ToListAsync();
        foreach (var order in orders)
        {
            order.OrderStatus = OrderStatus.SendToTechnicalExpert;
            await _repository.UpdateAsync(order, cancellationToken);
        }
        return 1;
    }

   
}