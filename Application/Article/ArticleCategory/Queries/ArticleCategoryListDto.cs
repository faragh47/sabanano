using AutoMapper;
using AutoMapper.Configuration;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class ArticleCategoryListDto : BaseDto<ArticleCategoryListDto, ArticleCategory, int>
{
    public string Title { get; set; }

    //public override void CustomMappings(IMappingExpression<ArticleCategory, ArticleCategoryListDto> mapping)
    //{
    //    mapping.ForMember(x=>x., opt => opt.Ignore());
    //}
}


public class ArticleCategoryBriefListDto : BaseDto<ArticleCategoryBriefListDto, ArticleCategory, int>
{
    public string Title { get; set; }
}
