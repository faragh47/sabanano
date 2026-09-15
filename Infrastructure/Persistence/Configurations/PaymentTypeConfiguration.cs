using System;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.HasData(PaymentType.CashWithAccount);
            builder.HasData(PaymentType.CashWithCredit);
            builder.HasData(PaymentType.Online);
            builder.HasData(PaymentType.CashOnDelivery);
            builder.HasData(PaymentType.PaymentWithAtthachment);
        }
    }

}