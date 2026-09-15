using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;
public class Transaction:BaseAuditableEntity<long>
{
    public long? PaymentId { get; set; }
    public bool IsDeposit { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Payment Payment { get; set; }
}
