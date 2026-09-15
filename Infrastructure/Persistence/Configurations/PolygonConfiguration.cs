using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;
internal class PolygonConfiguration : IEntityTypeConfiguration<Polygon>
{
    public void Configure(EntityTypeBuilder<Polygon> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(200);
        builder.Property(p => p.Comments).IsRequired(false).HasMaxLength(1000);
        builder.Property(p => p.FeatureJson).IsRequired();
        builder.Property(p => p.FeatureType).IsRequired().HasMaxLength(30);
        builder.HasOne(p => p.Address).WithMany(c => c.Polygons).HasForeignKey(c => c.AddressId);
    }
}
