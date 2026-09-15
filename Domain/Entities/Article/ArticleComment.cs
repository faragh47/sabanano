using System;
using CleanArchitecture.Entities.Articles;

namespace CleanArchitecture.Domain.Entities.Articles
{
	public class ArticleComment:BaseAuditableEntity<int>
	{
		public int ArticleId { get; set; }
		public string Comment { get; set; }
		public string IssuerName { get; set; }
		public string IssuerEmail { get; set; }
        public Article Article { get; set; }
    }
}

