using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;
public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.LatinTitle).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(450);
        builder.Property(c => c.Code).IsRequired().HasMaxLength(6);
        builder.Property(c => c.Amount).IsRequired(false);
        builder.Property(p => p.Amount).HasPrecision(25, 6);
        builder.Property(p => p.Max).HasPrecision(25, 6);
        builder.Property(c => c.Percent).IsRequired(false);
        builder.Property(c => c.Max).IsRequired(false);
        builder.Property(c => c.DistcountTypeId).IsRequired();
        builder.HasOne(c => c.DiscountType).WithMany(p => p.Discounts).HasForeignKey(p => p.DistcountTypeId);
    }
}

public class DiscountIncreaseConfiguration : IEntityTypeConfiguration<DiscountIncrease>
{
    public void Configure(EntityTypeBuilder<DiscountIncrease> builder)
    {
        builder.Property(c => c.DistcountId).IsRequired();
        builder.Property(c => c.Percent).IsRequired();
        builder.HasOne(c => c.Discount).WithMany(p => p.Increases).HasForeignKey(p => p.DistcountId);
    }
}

public class DiscountUsedConfiguration : IEntityTypeConfiguration<DiscountUsed>
{
    public void Configure(EntityTypeBuilder<DiscountUsed> builder)
    {
        builder.Property(c => c.DiscountId).IsRequired();
        builder.Property(c => c.PersonId).IsRequired();
        builder.HasOne(c => c.Discount).WithMany(p => p.Useds).HasForeignKey(p => p.DiscountId);
        builder.HasOne(c => c.Person).WithMany(p => p.DiscountUseds).HasForeignKey(p => p.PersonId);
    }
}