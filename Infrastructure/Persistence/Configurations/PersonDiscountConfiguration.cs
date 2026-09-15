using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;
public class PersonDiscountConfiguration : IEntityTypeConfiguration<PersonDiscount>
{
    public void Configure(EntityTypeBuilder<PersonDiscount> builder)
    {
        builder.Property(c => c.DiscountId).IsRequired();
        builder.Property(c => c.PersonId).IsRequired();
        builder.HasOne(p => p.Discount).WithMany(c => c.PersonDiscounts).HasForeignKey(c => c.DiscountId);
        builder.HasOne(p => p.Person).WithMany(c => c.Discounts).HasForeignKey(c => c.PersonId);
    }
}
