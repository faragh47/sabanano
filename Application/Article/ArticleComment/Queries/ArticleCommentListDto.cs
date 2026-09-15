using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class ArticleCommentListDto : BaseAuditableDto<ArticleCommentListDto, ArticleComment, int>
{
    public string Comment { get; set; }
    public string IssuerName { get; set; }
    public string IssuerEmail { get; set; }
}

    public class ArticleCommentBriefDto : BaseDto<ArticleCommentBriefDto, ArticleComment, int>
{
		public string Comment { get; set; }
		public string IssuerName { get; set; }
		public string IssuerEmail { get; set; }
}
