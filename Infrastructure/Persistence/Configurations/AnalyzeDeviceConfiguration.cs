using System;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
	public class AnalyzeDeviceConfiguration : IEntityTypeConfiguration<AnalyzerDevice>
    {
        public void Configure(EntityTypeBuilder<AnalyzerDevice> builder)
        {
            builder.Property(p => p.Price).HasPrecision(18, 1);
            builder.Property(p => p.DiscountPercent).IsRequired(false);
        }
    }

    public class AnalyzeDeviceSampleConfiguration : IEntityTypeConfiguration<AnalyzerDeviceSample>
    {
        public void Configure(EntityTypeBuilder<AnalyzerDeviceSample> builder)
        {
            builder.HasOne(p => p.AnalyzerDevice).WithMany(c => c.Samples).HasForeignKey(c => c.AnalyerDeviceId);
            builder.HasOne(p => p.SampleCategory).WithMany(c => c.AnalyzerDevices).HasForeignKey(c => c.SampleCategoryId);
        }
    }

    public class SampleCategoryConfiguration : IEntityTypeConfiguration<SampleCategory>
    {
        public void Configure(EntityTypeBuilder<SampleCategory> builder)
        {
            foreach (var item in SampleCategory.Items)
            {
                builder.HasData(item);
            }
        }
    }

    public class AnalyzeDeviceServiceConfiguration : IEntityTypeConfiguration<AnalyzeDeviceService>
    {
        public void Configure(EntityTypeBuilder<AnalyzeDeviceService> builder)
        {
            builder.Property(p => p.Price).IsRequired(false);
            builder.Property(p => p.Price).HasPrecision(18, 1);
            builder.HasOne(p => p.AnalyzerDevice).WithMany(c => c.Services).HasForeignKey(c => c.AnalyzeDeviceId);
        }
    }

    public class AnalyzeDeviceResponseConfiguration : IEntityTypeConfiguration<AnalyzeDeviceResponse>
    {
        public void Configure(EntityTypeBuilder<AnalyzeDeviceResponse> builder)
        {
            builder.Property(p => p.ImageId).IsRequired(false);
            builder.HasOne(p => p.Image).WithMany(c => c.AnalyzeDeviceResponses).HasForeignKey(c => c.ImageId);
            builder.HasOne(p => p.AnalyzerDevice).WithMany(c => c.Responses).HasForeignKey(c => c.AnalyzeDeviceId);
        }
    }

    public class AnalyzeDeviceInputConfiguration : IEntityTypeConfiguration<AnalyzeDeviceInput>
    {
        public void Configure(EntityTypeBuilder<AnalyzeDeviceInput> builder)
        {
            builder.Property(p => p.TextBox).IsRequired(false);
            builder.Property(p => p.isList).IsRequired(false);
            builder.Property(p => p.IsCheckbox).IsRequired(false);
            builder.HasOne(p => p.AnalyzerDevice).WithMany(c => c.Inputs).HasForeignKey(c => c.AnalyzeDeviceId);
        }
    }

    public class AnalyzeDeviceAttributeConfiguration : IEntityTypeConfiguration<AnalyzeDeviceAttribute>
    {
        public void Configure(EntityTypeBuilder<AnalyzeDeviceAttribute> builder)
        {
            builder.HasOne(p => p.AnalyzerDevice).WithMany(c => c.Attributes).HasForeignKey(c => c.AnalyzeDeviceId);
        }
    }

    public class AnalyzeDeviceAttributeDetailConfiguration : IEntityTypeConfiguration<AnalyzeDeviceAttributeDetail>
    {
        public void Configure(EntityTypeBuilder<AnalyzeDeviceAttributeDetail> builder)
        {
            builder.HasOne(p => p.AnalyzeDeviceAttribute).WithMany(c => c.Details).HasForeignKey(c => c.AnalyzeAttributeId);
        }
    }

}

