using System;
using CleanArchitecture.Infrastructure.Common;

namespace CleanArchitecture.Application.PagesDto
{
	public class IndexDto : BaseViewModel
    {
		public List<ArticleDto> Articles { get; set; }
	}
}

