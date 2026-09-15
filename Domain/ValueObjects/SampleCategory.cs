using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.ValueObjects;
public class SampleCategory : ValueObjectWithTile<int>
{
    public ICollection<AnalyzerDeviceSample> AnalyzerDevices { get; set; }
    private SampleCategory(string title, int id)
    {
        Title = title;
        Id = id;
    }
    public SampleCategory()
    {
    }

    public static SampleCategory Powdery => new("پودری", 1);
    public static SampleCategory Liquid => new("مایع", 2);
    public static SampleCategory Polymery => new("بالک (پلیمری و غیر فلزی)", 3);
    public static SampleCategory Solution => new("محلول", 4);
    public static IEnumerable<SampleCategory> Items
    {
        get
        {
            yield return Powdery;
            yield return Liquid;
            yield return Polymery;
            yield return Solution;
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
