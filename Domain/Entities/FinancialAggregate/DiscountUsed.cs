using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;
public class DiscountUsed : BaseAuditableEntity<long>
{
    public long DiscountId { get; set; }
    public long PersonId { get; set; }
    public Person Person { get; set; }
    public Discount Discount { get; set; }
}
