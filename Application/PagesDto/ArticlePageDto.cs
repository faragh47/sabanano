using System;
using CleanArchitecture.Infrastructure.Common;

namespace CleanArchitecture.Application.PagesDto
{
    public class ArticlePageDto : BaseViewModel
    {
        public ArticleDto Article { get; set; }
        public List<ArticleBriefDto> RecentArticles { get; set; }
    }
}

