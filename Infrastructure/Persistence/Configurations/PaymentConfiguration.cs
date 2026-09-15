using System;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.ImageId).IsRequired(false);
            builder.Property(x => x.TypeId).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasPrecision(18, 1);
            builder.HasOne(p => p.Image).WithMany(c => c.Payments).HasForeignKey(c => c.ImageId);
            builder.HasOne(p => p.Type).WithMany(c => c.Payments).HasForeignKey(c => c.TypeId);
        }
    }
}