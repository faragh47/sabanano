using System;
using CleanArchitecture.Domain.Entities.HrManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.PersonId).IsRequired();
            builder.Property(x => x.CustomerCode).HasMaxLength(200);
            builder.HasOne(p => p.Person).WithMany(c => c.Customers).HasForeignKey(c => c.PersonId);
            builder.HasOne(p => p.Account).WithMany(c => c.Customers).HasForeignKey(c => c.AccountId);
        }
    }
}

