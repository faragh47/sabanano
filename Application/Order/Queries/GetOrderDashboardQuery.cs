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

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetOrderDashboardQuery : BaseRecordSearchDto, IRequest<DashboardDto>
{
    public Expression<Func<Order, bool>> GenerateExpression(GetOrderDashboardQuery dto)
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

public class
   GetOrderDashboardQueryHandler : IRequestHandler<GetOrderDashboardQuery, DashboardDto>
{
    private readonly IRepository<Order> _repository;
    public GetOrderDashboardQueryHandler(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public async Task<DashboardDto> Handle(GetOrderDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var totalOrders = await _repository.TableNoTracking.CountAsync();
        var waitingOrders = await _repository.TableNoTracking.Where(x => x.OrderStatus.Id == OrderStatus.SendToTechnicalExpert.Id).CountAsync();

        return new DashboardDto
        {
            TotalOrdersCount = totalOrders,
            WaitingOrdersCount = waitingOrders
        };
    }
}