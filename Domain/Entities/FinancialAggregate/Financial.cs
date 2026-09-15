using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;

public class Financial : BaseAuditableEntity<long>
{
    public long? PaymentId { get; set; }
    public string Title { get; set; }
    [Precision(18, 2)] public decimal TotalPayablePrice { get; set; }
    [Precision(18, 2)] public decimal GrantPrice { get; set; }
    [Precision(18, 2)] public decimal AnalyzePrice { get; set; }
    [Precision(18, 2)] public decimal AdditionalPrice { get; set; }
    [Precision(18, 2)] public decimal Tax { get; set; }
    public decimal TotalPrice => (AnalyzePrice * Tax / 100m) + AdditionalPrice + AnalyzePrice;
    public int StatusId { get; set; }
    public Payment Payment { get; set; }
    public FinancialStatus Status { get; set; }
    public ICollection<FinancialDetail> Details { get; set; }
    public ICollection<Orders.Order> Orders { get; set; }
}