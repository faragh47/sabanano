using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Entities.Articles;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record GetArticlePageQuery : BaseRecordSearchDto, IRequest<ArticlePageDto>
{
    public int Id { get; set; }
    public int ExeptId { get; set; }

    public Expression<Func<Article, bool>> GenerateExpression(GetArticlePageQuery dto)
    {
        List<Expression<Func<Article, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);

        if (ExeptId > 0)
        {
            expressions.Add(src => src.Id != (ExeptId));
        }
        else if (Id > 0)
        {
            expressions.Add(src => src.Id == (Id));
        }


        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetArticleListQueryHandler : IRequestHandler<GetArticlePageQuery, ArticlePageDto>
{
    private readonly IRepository<Article> _repository;
    private readonly IMapper _mapper;

    public GetArticleListQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Article> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ArticlePageDto> Handle(GetArticlePageQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var resultArticle = await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        request.ExeptId = request.Id;
        request.RecordsPerPage = 4;
        expresion = request.GenerateExpression(request);

        var recentArticles = await _repository.TableNoTracking
            .Where(expresion)
            .ProjectTo<ArticleBriefDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var result = new ArticlePageDto()
        {
            Article = resultArticle,
            RecentArticles = recentArticles
        };

        return result;
    }
}
