using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetArticleCategoryWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<ArticleCategoryBriefListDto>>
{
    public string Title { get; set; }
    public Expression<Func<ArticleCategory, bool>> GenerateExpression(GetArticleCategoryWithPaginationQuery dto)
    {
        List<Expression<Func<ArticleCategory, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Contains(Title));
        }


        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetArticleCategoryWithPaginationQueryHandler : IRequestHandler<GetArticleCategoryWithPaginationQuery, PaginatedList<ArticleCategoryBriefListDto>>
{
    private readonly IRepository<ArticleCategory> _repository;
    private readonly IMapper _mapper;

    public GetArticleCategoryWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<ArticleCategory> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<ArticleCategoryBriefListDto>> Handle(GetArticleCategoryWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<ArticleCategoryBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
