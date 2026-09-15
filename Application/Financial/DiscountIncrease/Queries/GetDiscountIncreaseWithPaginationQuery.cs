using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetDiscountIncreaseWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<DiscountIncreaseBriefListDto>>
{

    public long? DistcountId { get; set; }
    public int? Percent { get; set; }
    public Expression<Func<DiscountIncrease, bool>> GenerateExpression(GetDiscountIncreaseWithPaginationQuery dto)
    {
        List<Expression<Func<DiscountIncrease, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }
        if (DistcountId is not null)
        {
            expressions.Add(src => src.DistcountId.Equals(DistcountId));
        }
        if (Percent is not null)
        {
            expressions.Add(src => src.Percent.Equals(Percent));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetDiscountIncreaseWithPaginationQueryHandler : IRequestHandler<GetDiscountIncreaseWithPaginationQuery, PaginatedList<DiscountIncreaseBriefListDto>>
{
    private readonly IRepository<DiscountIncrease> _repository;
    private readonly IMapper _mapper;

    public GetDiscountIncreaseWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<DiscountIncrease> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<DiscountIncreaseBriefListDto>> Handle(GetDiscountIncreaseWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<DiscountIncreaseBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
