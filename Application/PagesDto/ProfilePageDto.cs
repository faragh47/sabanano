using CleanArchitecture.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;

namespace CleanArchitecture.Application.PagesDto
{
    public class ProfilePageDto : BaseViewModel
    {
        public PeopleBriefDto Person { get; set; }
        public ComplaintDto Complaint { get; set; }
        public RecommendationDto Recommendation { get; set; }
        public List<ArticleDto> Articles { get; set; } = new();
        public DashboardDto Dashboard { get; set; } = new();
        public List<AnalyzeDevicePriceDto> AnalyzePrices { get; set; } = new();
    }

    public class DashboardDto
    {
        public long TotalOrdersCount { get; set; }
        public long WaitingOrdersCount { get; set; }
    }
    public class AnalyzeDevicePriceDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
    }
}