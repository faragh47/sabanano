using System;
using CleanArchitecture.Domain.Entities.HrManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            //builder.Property(p => p.Latitude).HasPrecision(10, 10);
            //builder.Property(p => p.Longitude).HasPrecision(10, 10);
            builder.Property(p => p.PostalCode).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.FullAddress).IsRequired();
            builder.Property(p => p.CityId).IsRequired(false);
            builder.HasOne(p => p.City).WithMany(c => c.Addresses).HasForeignKey(c => c.CityId);
        }
    }
}

