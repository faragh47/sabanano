using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.ValueObjects;
public class MaintenanceCondition : ValueObjectWithTile<int>
{
    //public ICollection<AnalyzerDevice> AnalyzerDevices { get; set; }
    private MaintenanceCondition(string title, int id)
    {
        Title = title;
        Id = id;
    }
    public MaintenanceCondition()
    {
    }

    public static MaintenanceCondition Light => new("حساس به نور", 1);
    public static MaintenanceCondition Liquid => new("حساس به رطوبت", 2);
    public static MaintenanceCondition Temperature => new("نگهداری در دمای پایین", 3);
    public static IEnumerable<MaintenanceCondition> Items
    {
        get
        {
            yield return Light;
            yield return Liquid;
            yield return Temperature;
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
