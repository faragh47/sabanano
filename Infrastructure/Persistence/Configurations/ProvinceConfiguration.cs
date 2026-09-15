using System;
using CleanArchitecture.Domain.Entities.BasicInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.HasOne(c => c.Country).WithMany(p => p.Provinces).HasForeignKey(p => p.CountryId);
        }
    }
}

