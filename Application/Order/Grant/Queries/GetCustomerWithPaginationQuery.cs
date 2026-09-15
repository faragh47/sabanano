using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.Grants;
using CleanArchitecture.Domain.Entities.Grants;
using CleanArchitecture.Domain.Entities.Order;
using Data.Contracts;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.Grants;

public record GetGrantWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<GrantBriefDto>>
{
    public Expression<Func<Grant, bool>> GenerateExpression(GetGrantWithPaginationQuery dto)
    {
        List<Expression<Func<Grant, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }
        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetGrantWithPaginationQueryHandler : IRequestHandler<GetGrantWithPaginationQuery, PaginatedList<GrantBriefDto>>
{
    private readonly IRepository<Grant> _repository;
    private readonly IMapper _mapper;

    public GetGrantWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Grant> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<GrantBriefDto>> Handle(GetGrantWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<GrantBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
