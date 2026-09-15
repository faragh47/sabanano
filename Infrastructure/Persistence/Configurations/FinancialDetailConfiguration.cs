using System;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class FinancialDetailConfiguration : IEntityTypeConfiguration<FinancialDetail>
    {
        public void Configure(EntityTypeBuilder<FinancialDetail> builder)
        {
            builder.Property(x => x.FinancialId).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasPrecision(25, 6);
            builder.HasOne(p => p.Financial).WithMany(c => c.Details).HasForeignKey(c => c.FinancialId);
        }
    }
}

