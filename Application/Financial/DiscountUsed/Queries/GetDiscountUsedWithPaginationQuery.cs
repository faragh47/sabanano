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

public record GetDiscountUsedWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<DiscountUsedBriefListDto>>
{

    public long? DiscountId { get; set; }
    public long? PersonId { get; set; }
    public Expression<Func<DiscountUsed, bool>> GenerateExpression(GetDiscountUsedWithPaginationQuery dto)
    {
        List<Expression<Func<DiscountUsed, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }
        if (DiscountId is not null)
        {
            expressions.Add(src => src.DiscountId.Equals(DiscountId));
        }
        if (PersonId is not null)
        {
            expressions.Add(src => src.PersonId.Equals(PersonId));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetDiscountUsedWithPaginationQueryHandler : IRequestHandler<GetDiscountUsedWithPaginationQuery, PaginatedList<DiscountUsedBriefListDto>>
{
    private readonly IRepository<DiscountUsed> _repository;
    private readonly IMapper _mapper;

    public GetDiscountUsedWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<DiscountUsed> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<DiscountUsedBriefListDto>> Handle(GetDiscountUsedWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<DiscountUsedBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
