using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.Orders.Queries;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetOrderWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<OrderBriefDto>>
{
    public OrderStatus OrderStatus { get; set; }

    public Expression<Func<Order, bool>> GenerateExpression(GetOrderWithPaginationQuery dto)
    {
        List<Expression<Func<Order, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (OrderStatus is not null && OrderStatus?.Id is not null)
        {
            expressions.Add(src => src.OrderStatus.Id == OrderStatus.Id);
        }

        if (CreatorId > 0)
        {
            expressions.Add(src => src.CreatedBy.Equals(CreatorId));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class
    GetOrderWithPaginationQueryHandler : IRequestHandler<GetOrderWithPaginationQuery, PaginatedList<OrderBriefDto>>
{
    private readonly IRepository<Order> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetOrderWithPaginationQueryHandler(IRepository<Order> repository,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<OrderBriefDto>> Handle(GetOrderWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsInRole("Admin"))
        {
            request.CreatorId = _currentUser.UserId;
        }

        var expresion = request.GenerateExpression(request);
        var OrderResult = await _repository.TableNoTracking
            .Include(x => x.Histories)
            .Include(x => x.Person)
            .Include(x => x.Image)
            .Include(x => x.OrderStatus)
            .Include(x => x.OrderAnalyzes)
            .ThenInclude(x => x.AnalyzerDevice)
            .Include(x => x.Financial)
            .ThenInclude(x => x.Payment)
            .ThenInclude(x => x.Image)
            .Where(expresion)
            .OrderByDescending(x => x.Created)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);
        List<OrderBriefDto> items = new();
        foreach (var item in OrderResult.Items)
        {
            var dto = _mapper.Map<OrderBriefDto>(item);
            dto.CreatedBy = $"{item.Person?.FirstName} {item.Person?.LastName}";
            if (item.Histories is { Count: > 0 })
            {
                dto.LastHistory = _mapper.Map<OrderHistoryDto>
                (item.Histories?
                    .OrderByDescending(x => x.Created)
                    .FirstOrDefault());
            }
            if (dto.Financial is null)
            {
                dto.Financial = new FinancialListDto()
                {
                    AnalyzePrice = item.OrderAnalyzes.Sum(x => x.AnalyzerDevice.Price)
                };
            }
            items.Add(dto);
        }

        PaginatedList<OrderBriefDto> result = new PaginatedList<OrderBriefDto>(items, OrderResult.TotalCount,
            (int)request.PageNumber,
            (int)request.RecordsPerPage);
        return result;
    }
}