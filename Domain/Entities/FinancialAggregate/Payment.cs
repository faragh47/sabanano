using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;
public class Payment : BaseAuditableEntity<long>
{
    public long? ImageId { get; set; }
    public int TypeId { get; set; }
    public bool IsApproved { get; set; }
    public decimal Price { get; set; }
    public PaymentType Type { get; set; }
    public Image Image { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Financial> Financials { get; set; }
}
