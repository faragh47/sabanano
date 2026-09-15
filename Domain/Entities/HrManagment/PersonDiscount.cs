using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.Entities.HrManagment;
public class PersonDiscount : BaseAuditableEntity<long>
{
    public long PersonId { get; set; }
    public long DiscountId { get; set; }
    public Discount Discount { get; set; }
    public Person Person { get; set; }
}
