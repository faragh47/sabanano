using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.ValueObjects;
public class FinancialStatus : ValueObjectWithTile<int>
{
    public ICollection<Financial> Financials { get; set; }
    //public ICollection<Service> Services { get; set; }
    private FinancialStatus(string title, int id)
    {
        Title = title;
        Id = id;
    }
    public FinancialStatus()
    {
    }

    public static FinancialStatus WaitForPayment => new("در انتظار پرداخت", 1);
    public static FinancialStatus Payed => new("پرداخت شده", 2);
    public static FinancialStatus CashOnDelivery => new("پرداخت حضوری", 3);
    public static IEnumerable<FinancialStatus> Items
    {
        get
        {
            yield return WaitForPayment;
            yield return Payed;
            yield return CashOnDelivery;
        }
    }

    public static long FindId(string title)
    {
      var item= GetItem(title, Items);
      return Convert.ToInt64(item);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
    }
}
