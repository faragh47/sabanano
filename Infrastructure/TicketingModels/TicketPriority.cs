using Common.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using static Common.Utilities.GlobalEnums;
using CleanArchitecture.Domain.Common;

namespace Entities.DatabaseModels.TicketingModels
{
    public class TicketPriority : BaseAuditableEntity<int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public int ResponseTime { get; set; }

        //public ICollection<TikTicket> Tickets { get; set; }
        public ICollection<TicketCategory> TicketCategories { get; set; }

    }

    public class TicketPriorityConfiguration : IEntityTypeConfiguration<TicketPriority>
    {
        public void Configure(EntityTypeBuilder<TicketPriority> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LatinTitle).HasMaxLength(100).HasColumnType("varchar");

            foreach (var ticketPriority in (GlobalEnums.TicketPriority[])Enum.GetValues(typeof(GlobalEnums.TicketPriority)))
            {
                var LatinTitle = Enum.GetName(typeof(GlobalEnums.TicketPriority), ticketPriority);
                var title = ticketPriority.ToDisplay();

                builder.HasData(new TicketPriority
                {
                    Id = (int)ticketPriority,
                    Title = title,
                    LatinTitle = LatinTitle,
                    IsActive = true,
                    CreatedBy = 2,
                    LastModifiedBy = 2,
                    Created = new DateTime(2021, 1, 1),
                    LastModified = new DateTime(2021, 1, 1)
                });
            }
        }
    }
}
