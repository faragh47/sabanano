using System;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class FinancialConfiguration : IEntityTypeConfiguration<Financial>
    {
        public void Configure(EntityTypeBuilder<Financial> builder)
        {
            builder.Property(x => x.StatusId).IsRequired();
            builder.Property(x => x.PaymentId).IsRequired(false);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(p => p.TotalPayablePrice).IsRequired().HasPrecision(18, 1);
            builder.HasOne(p => p.Payment).WithMany(c => c.Financials).HasForeignKey(c => c.PaymentId);
            builder.HasOne(p => p.Status).WithMany(c => c.Financials).HasForeignKey(c => c.StatusId);
        }
    }
}