using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.ValueObjects;
public class PaymentType : ValueObjectWithTile<int>
{
    public ICollection<Payment> Payments { get; set; }
    //public ICollection<Service> Services { get; set; }
    private PaymentType(string title, int id)
    {
        Title = title;
        Id = id;
    }
    public PaymentType()
    {
    }

    public static PaymentType Online => new("پرداخت انلاین", 1);
    public static PaymentType PaymentWithAtthachment => new("واریز مستقیم", 2);
    public static PaymentType CashOnDelivery => new("پرداخت حضوری", 3);
    public static PaymentType CashWithAccount => new("پرداخت با کیف پول", 4);
    public static PaymentType CashWithCredit => new("پرداخت اعتباری", 5);
    public static IEnumerable<PaymentType> Items
    {
        get
        {
            yield return Online;
            yield return PaymentWithAtthachment;
            yield return CashOnDelivery;
            yield return CashWithCredit;
            yield return CashWithAccount;
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
