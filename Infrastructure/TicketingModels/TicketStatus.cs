using Common.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using static Common.Utilities.GlobalEnums;
using CleanArchitecture.Domain.Common;

namespace Entities.DatabaseModels.TicketingModels
{
    public class TicketStatus : BaseAuditableEntity<int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TicketStatusHistory> TicketStatusHistories { get; set; }
    }

    public class TicketStatusConfiguration : IEntityTypeConfiguration<TicketStatus>
    {
        public void Configure(EntityTypeBuilder<TicketStatus> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LatinTitle).HasMaxLength(100).HasColumnType("varchar");
            
            foreach (var ticketStatus in (GlobalEnums.TicketStatus[])Enum.GetValues(typeof(GlobalEnums.TicketStatus)))
            {
                var LatinTitle = Enum.GetName(typeof(GlobalEnums.TicketStatus), ticketStatus);
                var title = ticketStatus.ToDisplay();

                builder.HasData(new TicketStatus
                {
                    Id = (int)ticketStatus,
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
