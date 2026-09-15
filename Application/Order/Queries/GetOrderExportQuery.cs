using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination.OrderBriefDto;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetOrderExportQuery : BaseRecordSearchDto, IRequest<List<OrderExportDto>>
{
    public Expression<Func<Order, bool>> GenerateExpression(GetOrderExportQuery dto)
    {
        List<Expression<Func<Order, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (CreatorId > 0)
        {
            expressions.Add(src => src.CreatedBy.Equals(CreatorId));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetOrderExportQueryHandler : IRequestHandler<GetOrderExportQuery, List<OrderExportDto>>
{
    private readonly IRepository<Order> _repository;
    public GetOrderExportQueryHandler(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public async Task<List<OrderExportDto>> Handle(GetOrderExportQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _repository.TableNoTracking
                            .Include(x => x.Histories)
                            .Include(x => x.Financial)
                            .ThenInclude(x => x.Payment)
                            .Include(x => x.Person)
                            .ThenInclude(x => x.MobileNumbers)
                            .Include(x => x.OrderAnalyzes)
                            .ThenInclude(x => x.AnalyzerDevice)
                            .Where(x => x.Histories.Any(x => x.OrderStatus.Id == OrderStatus.TechnicalConfirm.Id)).ToListAsync();

        var result = new List<OrderExportDto>();
        foreach (var item in orders)
        {
            var orderAnalyze = item.OrderAnalyzes.FirstOrDefault();
            result.Add(new OrderExportDto
            {
                RowIndex = item.Id,
                SampleCode = item.TrackingCode,
                Analysis = orderAnalyze?.Name ?? "",
                Count = item.OrderAnalyzes.Count(),
                CustomerName = string.Concat(item.Person?.FirstName, " ", item.Person?.LastName),
                Date = item.Created.ToPersianDate(),
                Description = item.Description,
                DocCode = orderAnalyze?.AnalyzerDevice.DocumentCode,
                NationalId = item.Person?.NationalId,
                AnalysisCost = item.Financial?.AnalyzePrice ?? decimal.Zero,
                NetAfterGrantTax = item.Financial?.TotalPayablePrice ?? decimal.Zero,
                TotalCost = item.Financial?.TotalPrice ?? decimal.Zero,
                TotalPaid = item.Financial?.Payment?.IsApproved is true ? item.Financial.TotalPayablePrice : 0,
                PaidOrNot = item.Financial?.Payment?.IsApproved is true ? "پرداخت شده" : "پرداخت نشده",
                Tax = item.Financial?.Tax ?? decimal.Zero,
                PartnerLab = "",
                PartnerPaid = 0,
                Phone = item.Person?.MobileNumbers?.FirstOrDefault()?.MobileNumber ?? "",
                ResultSent = item.Histories.Any(x => x.OrderStatus.Id == OrderStatus.Completed.Id) ? "ارسال شده" : "ارسال نشده",
                Email = "",
                SetadPaid = item.Financial?.GrantPrice ?? decimal.Zero,
                StatusPaid = item.Financial?.StatusId == FinancialStatus.Payed.Id ? "پرداخت شده" : "پرداخت نشده",
                TypeNormalUrgent = "عادی",
            });
        }
        return result;
    }
}