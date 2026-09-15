using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.ValueObjects;
public class DiscountType : ValueObjectWithEntity<int>
{
    public ICollection<Discount> Discounts { get; set; }

    private DiscountType(string title, int id)
    {
        Title = title;
        Id = id;
        IsActive = true;
    }

    public static DiscountType Restaurant => new("Restaurant", 1);
    public static DiscountType Total => new("Total", 2);
    public static IEnumerable<DiscountType> Items
    {
        get
        {
            yield return Restaurant;
            yield return Total;
        }
    }

    public static int FindId(string title)
    {
        var item = GetItem(title);
        return Convert.ToInt32(item);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
    }

    public static DiscountType? GetItem(string item)
    {
        var result = Items.Where(x => x.Title == item).FirstOrDefault();
        return result;
    }
}
