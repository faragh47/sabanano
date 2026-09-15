using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;
public class DiscountIncrease:BaseAuditableEntity<long>
{
    public long DistcountId { get; set; }
    public int Percent { get; set; }
    public Discount Discount { get; set; }
}
