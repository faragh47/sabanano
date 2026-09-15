using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Entities.Articles;

namespace CleanArchitecture.Domain.ValueObjects;
public class ArticleCategory : ValueObjectWithTile<int>
{
    public ICollection<Article> Articles { get; set; }
    //public ICollection<Service> Services { get; set; }
    private ArticleCategory(string title, int id)
    {
        Title = title;
        Id = id;
    }
    public ArticleCategory()
    {
    }

    public static ArticleCategory Prepration => new("آماده سازی", 1);
    public static ArticleCategory Device => new("آنالیزها", 2);
    public static IEnumerable<ArticleCategory> Items
    {
        get
        {
            yield return Prepration;
            yield return Device;
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
