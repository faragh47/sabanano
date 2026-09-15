using System;
using CleanArchitecture.Application.Article;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Entities.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(101).IsRequired();
            builder.Property(p => p.HeaderName).IsRequired(false);
            builder.HasOne(p => p.Image).WithMany(c => c.Articles).HasForeignKey(c => c.ImageId);
            builder.HasOne(p => p.Category).WithMany(c => c.Articles).HasForeignKey(c => c.CategoryId);
        }
    }

    public class ArticleCommentConfiguration : IEntityTypeConfiguration<ArticleComment>
    {
        public void Configure(EntityTypeBuilder<ArticleComment> builder)
        {
            builder.Property(p => p.Comment).IsRequired();
            builder.Property(p => p.IssuerName).HasMaxLength(100);
            builder.Property(p => p.IssuerEmail).HasMaxLength(100);
            builder.HasOne(p => p.Article).WithMany(c => c.ArticleComments).HasForeignKey(c => c.ArticleId);
        }
    }

    public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            foreach (var item in ArticleCategory.Items)
            {
                builder.HasData(item);
            }
        }
    }

    public class ArticleDetailConfiguration : IEntityTypeConfiguration<ArticleDetail>
    {
        public void Configure(EntityTypeBuilder<ArticleDetail> builder)
        {
            builder.Property(p => p.ImageId).IsRequired(false);
            builder.Property(p => p.Paragraph).IsRequired(false);
            builder.Property(p => p.HeaderName).IsRequired(false);
            builder.HasOne(p => p.Image).WithMany(c => c.ArticleDetails).HasForeignKey(c => c.ImageId);
            builder.HasOne(p => p.Article).WithMany(c => c.ArticleDetails).HasForeignKey(c => c.ArticleId);
        }
    }
}

