using System;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class PersonMobileNumberConfiguration : IEntityTypeConfiguration<PersonMobileNumber>
    {
        public void Configure(EntityTypeBuilder<PersonMobileNumber> builder)
        {
            builder.Property(p => p.MobileNumber).IsRequired().HasMaxLength(12); //like 989122106519
            builder.HasOne(p => p.Person).WithMany(c => c.MobileNumbers).HasForeignKey(p => p.PersonId);
        }
    }
}

