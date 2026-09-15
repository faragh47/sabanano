using System;
using CleanArchitecture.Domain.Entities.BasicInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.LatinName).HasMaxLength(200);
        }
    }
}

