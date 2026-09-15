using System;
using CleanArchitecture.Domain.Entities.HrManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class CrePeopleAddressesConfiguration : IEntityTypeConfiguration<PeopleAddress>
    {
        public void Configure(EntityTypeBuilder<PeopleAddress> builder)
        {
            builder.Property(p => p.AddressId).IsRequired();
            builder.Property(p => p.PersonId).IsRequired();
            builder.HasIndex(x => new { x.AddressId, x.PersonId }).IsUnique();
            builder.Property(p => p.IsDefault).IsRequired();
            builder.HasOne(p => p.Person).WithMany(c => c.PeopleAddresses).HasForeignKey(c => c.PersonId);
            builder.HasOne(p => p.Address).WithMany(c => c.PeopleAddresses).HasForeignKey(c => c.AddressId);
        }
    }
}

