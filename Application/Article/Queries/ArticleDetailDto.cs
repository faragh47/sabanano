using AutoMapper;
using AutoMapper.Configuration;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Article.Queries
{
    public class ArticleDetailDto : BaseDto<ArticleDetailDto, ArticleDetail, int>
    {
        public string Paragraph { get; set; }
        public string HeaderName { get; set; }
        public bool IsBulletPoint { get; set; }
        public long? ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int OrderBy { get; set; }

        public override void CustomMappings(IMappingExpression<ArticleDetail, ArticleDetailDto> mapping)
        {
            mapping.ForMember(d => d.ImageUrl, o =>
             {
                 o.PreCondition(src => src.Image != null);
                 o.MapFrom(src => "../img/article/" + src.Image.FileName);
             });
        }
    }
}
