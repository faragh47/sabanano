using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;
public class FinancialDetail:BaseAuditableEntity<long>
{
    public long FinancialId { get; set; }
    public decimal Price { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Financial Financial { get; set; }
}
