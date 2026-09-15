using System;
using System.CodeDom;
using System.Reflection.Emit;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(m => m.CountryId).IsRequired(false);
            builder.Property(m => m.HomeTownCityId).IsRequired(false);
            builder.Property(m => m.ImageId).IsRequired(false);
            builder.Property(m => m.GenderTypeId).IsRequired(false);
            builder.Property(m => m.Birthday).IsRequired(false);
            builder.Property(p => p.FirstName).HasMaxLength(200);
            builder.Property(p => p.LastName).HasMaxLength(200);
            builder.Property(p => p.FatherName).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.NationalId).HasMaxLength(10).IsRequired(false);
            builder.HasOne(p => p.Nationality).WithMany(c => c.People).HasForeignKey(c => c.CountryId);
            builder.HasOne(p => p.HomeTownCity).WithMany(c => c.People).HasForeignKey(c => c.HomeTownCityId);
            builder.HasOne(p => p.GenderType).WithMany(c => c.People).HasForeignKey(c => c.GenderTypeId);
            builder.HasOne(p => p.Image).WithMany(c => c.People).HasForeignKey(c => c.ImageId);
            //builder.HasOne(p => p.Person).WithMany(c => c.IndividualPeople).HasForeignKey(c => c.FPeopleId);
            //builder.HasOne(p => p.MaritalStatus).WithMany(c => c.Individuals).HasForeignKey(c => c.FMaritalStatusId);
            //builder.HasOne(p => p.MilitaryStatus).WithMany(c => c.Individuals).HasForeignKey(c => c.FMilitaryStatusId);
            //builder.HasOne(p => p.Religion).WithMany(c => c.Individuals).HasForeignKey(c => c.FReligionTypesId);
        }
    }
}

