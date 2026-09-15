using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.LatinTitle).HasMaxLength(200);
        builder.Property(c => c.FileName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.FileExt).IsRequired().HasMaxLength(6);
        builder.Property(c => c.SHA256).IsRequired(false).HasMaxLength(64);

    }
}
