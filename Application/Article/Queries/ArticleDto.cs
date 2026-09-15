using AutoMapper;
using CleanArchitecture.Application.Article.Queries;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ArticleDto : BaseAuditableDto<ArticleDto, Article, int>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string HeaderName { get; set; }
    public string CategoryTitle { get; set; }
    public List<ArticleDetailDto> ArticleDetails { get; set; }
    public List<ArticleCommentListDto> ArticleComments { get; set; }
    public override void CustomMappings(IMappingExpression<Article, ArticleDto> mapping)
    {
        mapping.ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => "../img/article/" + src.Image.FileName));
        mapping.ForMember(dest => dest.CategoryTitle, conf => conf.MapFrom(src =>  src.Category.Title));
        mapping.ForMember(dest => dest.ArticleDetails, conf => conf.MapFrom(src =>  src.ArticleDetails.OrderBy(x=>x.OrderBy)));
    }
}

public class ArticleBriefDto : BaseAuditableDto<ArticleBriefDto, Article, int>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string HeaderName { get; set; }
    public string CategoryTitle { get; set; }
    public override void CustomMappings(IMappingExpression<Article, ArticleBriefDto> mapping)
    {
        mapping.ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => "../img/article/" + src.Image.FileName));
        mapping.ForMember(dest => dest.CategoryTitle, conf => conf.MapFrom(src => src.Category.Title));
    }
}

