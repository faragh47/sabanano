using System;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.TrackingCode).IsRequired();
            builder.HasOne(p => p.Grant)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.GrantId);
            builder.HasOne(p => p.Person)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.PersonId);
            builder.HasOne(p => p.Financial)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.FinancialId);
            builder.HasOne(p => p.Image)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.ImageId);
            builder.OwnsOne(w => w.OrderStatus, wt =>
            {
                wt.Property(wt => wt.Id).HasColumnName("StatusId").HasDefaultValue(OrderStatus.Initial.Id);
                wt.Property(wt => wt.Title).HasColumnName("StatusTitle").HasDefaultValue(OrderStatus.Initial.Title);
            });
        }
    }

    public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
    {
        public void Configure(EntityTypeBuilder<OrderHistory> builder)
        {
            builder.HasOne(p => p.Order)
                .WithMany(c => c.Histories);
            builder.OwnsOne(w => w.OrderStatus, wt =>
            {
                wt.Property(wt => wt.Id).HasColumnName("StatusId").HasDefaultValue(OrderStatus.Initial.Id);
                wt.Property(wt => wt.Title).HasColumnName("StatusTitle").HasDefaultValue(OrderStatus.Initial.Title);
            });
        }
    }

    public class OrderSurveyConfiguration : IEntityTypeConfiguration<OrderSurvey>
    {
        public void Configure(EntityTypeBuilder<OrderSurvey> builder)
        {
            builder.HasOne(p => p.Order).WithMany(c => c.Surveys);

                builder.OwnsOne(w => w.Score, wt =>
                {
                    wt.Property(wt => wt.Id).HasColumnName("ScoreId").HasDefaultValue(SurveyScore.Average.Id);
                    wt.Property(wt => wt.Title).HasColumnName("ScoreTitle").HasDefaultValue(SurveyScore.Average.Title);
                });
        }
    }

    public class OrderAnalyzeConfiguration : IEntityTypeConfiguration<OrderAnalyze>
    {
        public void Configure(EntityTypeBuilder<OrderAnalyze> builder)
        {
            builder.HasOne(p => p.Order)
                .WithMany(c => c.OrderAnalyzes)
                .HasForeignKey(c => c.OrderId);
            builder.HasOne(p => p.AnalyzerDevice)
                .WithMany(c => c.OrderAnalyzes)
                .HasForeignKey(c => c.AnalyzeDeviceId);
            builder.HasOne(p => p.Safety)
                .WithMany(c => c.OrderAnalyzes)
                .HasForeignKey(c => c.SafetyId);
        }
    }

    public class BetConfiguration : IEntityTypeConfiguration<BET>
    {
        public void Configure(EntityTypeBuilder<BET> builder)
        {
        }
    }
    public class AASConfiguration : IEntityTypeConfiguration<AAS>
    {
        public void Configure(EntityTypeBuilder<AAS> builder)
        {
        }
    }
    public class AFMConfiguration : IEntityTypeConfiguration<AFM>
    {
        public void Configure(EntityTypeBuilder<AFM> builder)
        {
        }
    }
    public class ContactAngleConfiguration : IEntityTypeConfiguration<ContactAngle>
    {
        public void Configure(EntityTypeBuilder<ContactAngle> builder)
        {
        }
    }
}