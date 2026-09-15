using System.Globalization;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateOrder;

public record ConfirmFinancialCommand : BaseRecordDto<ConfirmFinancialCommand, Order, long>,
    IRequest<long>
{
    public string TechnicalComment { get; set; }
    public double ProcessingTime { get; set; }

    public override void CustomMappings(IMappingExpression<ConfirmFinancialCommand, Order> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class ConfirmFinancialCommandHandler : IRequestHandler<ConfirmFinancialCommand, long>
{
    private readonly IRepository<Order> _repository;
    private readonly IMediator _mediator;
    private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
    private const long LASTMAXORDERID = 1000;
    private readonly IRepository<AnalyzerDevice> _AnalyzeDeviceRepository;

    public ConfirmFinancialCommandHandler(IRepository<Order> repository,
        IMediator mediator, IRepository<AnalyzerDevice> analyzeDeviceRepository)
    {
        _repository = repository;
        _mediator = mediator;
        _AnalyzeDeviceRepository = analyzeDeviceRepository;
    }
    public async Task<long> Handle(ConfirmFinancialCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.TableNoTracking
            .Include(x=>x.OrderAnalyzes)
            .Include(x=>x.Financial)
            .ThenInclude(x=>x.Payment)
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync(x => x.OrderStatus.Id == OrderStatus.WaitingForFinancial.Id
                                      && x.Id == request.Id);
        await _mediator.Send(new CreateOrderHistoryCommand()
        {
            OrderStatus = OrderStatus.FinancialConfirmed,
            OrderId = request.Id,
            TechnicalComment = request.TechnicalComment
        });
        order.OrderStatus = OrderStatus.FinancialConfirmed;
        order.Financial.Payment.IsApproved=true;
        order.ProcessingTime = request.ProcessingTime;
        order.Financial.StatusId=FinancialStatus.Payed.Id;
        await GeneratedCode(order);
        await _repository.UpdateAsync(order, cancellationToken);
        return order.Id;
    }
    private async Task GeneratedCode(Order order)
    {
        var persianCalendar = new PersianCalendar();
        int shamsiYear = persianCalendar.GetYear(DateTime.Now);
        int shamsiMonth = persianCalendar.GetMonth(DateTime.Now);
        string shamsiYearTwoDigits = (shamsiYear % 100).ToString("D2");
        string shamsiMonthTwoDigits = (shamsiMonth % 100).ToString("D2");
        var orderAnaylyze = order.OrderAnalyzes.First();
        var code =await _AnalyzeDeviceRepository.TableNoTracking
            .Where(x => x.Id == orderAnaylyze.AnalyzeDeviceId)
            .Select(x => x.Code).FirstOrDefaultAsync();
        semaphore.Wait();
        var count = await _repository.TableNoTracking.CountAsync();
        count++;
        order.OrderNumber = (LASTMAXORDERID + count).ToString();
        order.TrackingCode = $"{shamsiYearTwoDigits}{shamsiMonthTwoDigits}{(LASTMAXORDERID + count)}-{code}";
        semaphore.Release();
    }
}