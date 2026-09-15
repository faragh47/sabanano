using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.OrderHistorys;
using CleanArchitecture.Domain.Entities.Order;
using CleanArchitecture.Domain.Entities.Orders;
using Data.Contracts;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.OrderHistorys;

public record GetOrderHistoryWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<OrderHistoryListDto>>
{
    public Expression<Func<OrderHistory, bool>> GenerateExpression(GetOrderHistoryWithPaginationQuery dto)
    {
        List<Expression<Func<OrderHistory, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }
        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetOrderHistoryWithPaginationQueryHandler : IRequestHandler<GetOrderHistoryWithPaginationQuery, PaginatedList<OrderHistoryListDto>>
{
    private readonly IRepository<OrderHistory> _repository;
    private readonly IMapper _mapper;

    public GetOrderHistoryWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<OrderHistory> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<OrderHistoryListDto>> Handle(GetOrderHistoryWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<OrderHistoryListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
