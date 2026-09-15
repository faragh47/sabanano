using CleanArchitecture.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

namespace CleanArchitecture.Application.PagesDto
{
    public class OrderCartPageDto : BaseViewModel
    {
        public bool IsSendToTechnicalExpert { get; set; }
        public List<OrderBriefDto> Orders { get; set; }
        public List<ArticleDto> Articles { get; set; }
    }
}