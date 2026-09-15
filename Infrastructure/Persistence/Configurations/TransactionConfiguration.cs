using System;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Title).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasPrecision(25, 6);
            builder.HasOne(p => p.Payment).WithMany(c => c.Transactions).HasForeignKey(c => c.PaymentId);
        }
    }
}

