using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetArticleCommentWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<ArticleCommentBriefDto>>
{
    public string Comment { get; set; }

    public Expression<Func<ArticleComment, bool>> GenerateExpression(GetArticleCommentWithPaginationQuery dto)
    {
        List<Expression<Func<ArticleComment, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Comment))
        {
            expressions.Add(src => src.Comment.Equals(Comment));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetArticleCommentWithPaginationQueryHandler : IRequestHandler<GetArticleCommentWithPaginationQuery, PaginatedList<ArticleCommentBriefDto>>
{
    private readonly IRepository<ArticleComment> _repository;
    private readonly IMapper _mapper;

    public GetArticleCommentWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<ArticleComment> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<ArticleCommentBriefDto>> Handle(GetArticleCommentWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<ArticleCommentBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
