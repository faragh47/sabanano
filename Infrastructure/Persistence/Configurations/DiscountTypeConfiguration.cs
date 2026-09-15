using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Entities.BasicInformation;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;
public class DiscountTypeConfiguration : IEntityTypeConfiguration<DiscountType>
{
    public void Configure(EntityTypeBuilder<DiscountType> builder)
    {
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.HasData(DiscountType.Restaurant);
        builder.HasData(DiscountType.Total);
    }
}