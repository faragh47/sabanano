using CleanArchitecture.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.Orders.Queries;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities.Orders;

namespace CleanArchitecture.Application.PagesDto
{
    public class RegisterOrderPageDto : BaseViewModel
    {
        public long OrderId { get; set; }
        public string Name { get; set; }
        public OrderAnalyzeCommand OrderAnalyze { get; set; }
        public List<OrderAnalyzeCommand> OrderAnalyzes { get; set; } = new();
        public bool IsRequireToReturnSample { get; set; }
        public bool IsRequireHeader { get; set; }
        public bool HasGrant { get; set; }
        public bool HasCompany { get; set; }
        public string? Description { get; set; }
        public string OrderNumber { get; set; }
        public string TrackingCode { get; set; }
        public FinancialListDto Financial { get; set; }
        public CreateGrantCommand Grant { get; set; }
        public CreateCompanyCommand Company { get; set; }
        public CustomerInfo Customer { get; set; }
        public List<OrderHistoryDto> Histories { get; set; } = new();

        public RegisterOrderPageDto()
        {
            OrderAnalyze = new();
        }
    }
}

public class RegisterOrderLevel2PageDto : BaseViewModel
{
    public long OrderId { get; set; }
    public string Name { get; set; }
    public CreateGrantCommand Grant { get; set; }
    public CreateCompanyCommand Company { get; set; }

    public RegisterOrderLevel2PageDto()
    {
        Grant = new();
        Company = new();
    }
}