using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Entities.Articles
{
    public class Article : BaseAuditableEntity<int>
    {
        public string Name { get; set; }
        public long ImageId { get; set; }
        public int CategoryId { get; set; }
        public string HeaderName { get; set; }
        public Image Image { get; set; }
        public ArticleCategory Category { get; set; }
        public ICollection<ArticleDetail> ArticleDetails { get; set; }
        public ICollection<ArticleComment> ArticleComments { get; set; }
    }
}
