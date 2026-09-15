using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Entities.Articles
{
    public class ArticleDetail : BaseEntity<int>
    {
        public int ArticleId { get; set; }
        public int OrderBy { get; set; }
        public string Paragraph { get; set; }
        public string HeaderName { get; set; }
        public bool IsBulletPoint { get; set; }
        public long? ImageId { get; set; }
        public Image? Image { get; set; }
        public Article Article   { get; set; }
    }
}
