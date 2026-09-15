using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetPolygonWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<PolygonBriefDto>>
{
    public int? CityId { get; set; }
    public string Title { get; set; }
    public Expression<Func<Polygon, bool>> GenerateExpression(GetPolygonWithPaginationQuery dto)
    {
        List<Expression<Func<Polygon, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Equals(Title));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Contains(Title));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetPolygonWithPaginationQueryHandler : IRequestHandler<GetPolygonWithPaginationQuery, PaginatedList<PolygonBriefDto>>
{
    private readonly IRepository<Polygon> _repository;
    private readonly IMapper _mapper;

    public GetPolygonWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Polygon> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<PolygonBriefDto>> Handle(GetPolygonWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<PolygonBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
