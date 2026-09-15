using System;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class FinancialStatusConfiguration : IEntityTypeConfiguration<FinancialStatus>
    {
        public void Configure(EntityTypeBuilder<FinancialStatus> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.HasData(FinancialStatus.CashOnDelivery);
            builder.HasData(FinancialStatus.WaitForPayment);
            builder.HasData(FinancialStatus.Payed);
        }
    }

}