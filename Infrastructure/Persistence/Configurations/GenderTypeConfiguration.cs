using System;
using CleanArchitecture.Domain.Entities.BasicInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class GenderTypeConfiguration : IEntityTypeConfiguration<GenderType>
    {
        public void Configure(EntityTypeBuilder<GenderType> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(50);
            builder.Property(c => c.LatinTitle).IsRequired(false).HasMaxLength(50);

            var creationDate = new System.DateTime(2021, 1, 1);
            builder.HasData(new GenderType { Id = 1, Title = "زن", LatinTitle = "Woman", CreatedBy = 2, LastModifiedBy = 2, IsActive = true, Created = creationDate, LastModified = creationDate });
            builder.HasData(new GenderType { Id = 2, Title = "مرد", LatinTitle = "Man", CreatedBy = 2, LastModifiedBy = 2, IsActive = true, Created = creationDate, LastModified = creationDate });
        }
    }
}

