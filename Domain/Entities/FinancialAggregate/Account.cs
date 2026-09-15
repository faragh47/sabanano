using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate;
public class Account:BaseAuditableEntity<long>
{
    public string Title { get; set; }
    public string ShebaCode { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public decimal Debit { get; set; }
    public ICollection<Customer> Customers { get; set; }
}
