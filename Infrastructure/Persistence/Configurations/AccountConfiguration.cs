using System;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.ShebaCode).HasMaxLength(24);
            builder.Property(x => x.ShebaCode).IsRequired(false);
            builder.Property(p => p.Credit).IsRequired().HasPrecision(25, 6);
            builder.Property(p => p.Debit).IsRequired().HasPrecision(25, 6);
            builder.Property(p => p.Balance).IsRequired().HasPrecision(25, 6);
        }
    }
}

