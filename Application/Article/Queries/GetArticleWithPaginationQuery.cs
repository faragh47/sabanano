using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Entities.Articles;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;


public record GetArticleWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<ArticleDto>>
{
    public string Name { get; set; }

    public Expression<Func<Article, bool>> GenerateExpression(GetArticleWithPaginationQuery dto)
    {
        List<Expression<Func<Article, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Name))
        {
            expressions.Add(src => src.Name.Equals(Name));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetArticleWithPaginationQueryHandler : IRequestHandler<GetArticleWithPaginationQuery, PaginatedList<ArticleDto>>
{
    private readonly IRepository<Article> _repository;
    private readonly IMapper _mapper;

    public GetArticleWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Article> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<ArticleDto>> Handle(GetArticleWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
