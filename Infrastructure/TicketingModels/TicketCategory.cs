using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Common;

namespace Entities.DatabaseModels.TicketingModels
{
    public class TicketCategory : BaseAuditableEntity<int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public int TicketPriorityId { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public TicketPriority TicketPriority { get; set; }
    }

    public class TicketCategoryConfiguration : IEntityTypeConfiguration<TicketCategory>
    {
        public void Configure(EntityTypeBuilder<TicketCategory> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LatinTitle).HasMaxLength(100).HasColumnType("varchar");
            builder.HasOne(p => p.TicketPriority).WithMany(c => c.TicketCategories).HasForeignKey(p => p.TicketPriorityId);
        }
    }
}
