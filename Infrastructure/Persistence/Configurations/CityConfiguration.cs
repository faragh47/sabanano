using System;
using CleanArchitecture.Domain.Entities.BasicInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.LatinName).HasMaxLength(200);
            builder.HasOne(c => c.Province).WithMany(p => p.Cities).HasForeignKey(p => p.ProvinceId);
        }
    }
}

