using System;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class UserCodeConfiguration : IEntityTypeConfiguration<AccUserCode>
    {
        public void Configure(EntityTypeBuilder<AccUserCode> builder)
        {
            builder.Property(m => m.Code).HasMaxLength(6).IsRequired(true);
            builder.Property(m => m.UserId).IsRequired(true);
            builder.HasOne(p => p.User).WithMany(c => c.Codes).HasForeignKey(c => c.UserId);
        }
    }

}