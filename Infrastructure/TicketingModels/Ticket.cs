using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Identity;

namespace Entities.DatabaseModels.TicketingModels
{
    public class Ticket : BaseAuditableEntity<long>
    {
        public string Title { get; set; }
        public int TicketCategoryId { get; set; }
        //public int FTicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public long? ReferedUserId { get; set; }

        public TicketCategory TicketCategory { get; set; }
        //public TikTicketPriority TicketPriority { get; set; }
        public TicketStatus TicketStatus { get; set; }

        [ForeignKey("CreatorId")]
        public new ApplicationUser Creator { get; set; }
        [ForeignKey("ReferedUserId")]
        public ApplicationUser ReferedUser { get; set; }
        public ICollection<TicketUserResponse> TicketUserResponses { get; set; }
        public ICollection<TicketReferHistory> TicketRefers { get; set; }
        public ICollection<TicketStatusHistory> TicketStatusHistories { get; set; }
    }

    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ReferedUserId).IsRequired(false);

            builder.HasOne(p => p.TicketCategory).WithMany(c => c.Tickets).HasForeignKey(p => p.TicketCategoryId);
            //builder.HasOne(p => p.TicketPriority).WithMany(c => c.Tickets).HasForeignKey(p => p.FTicketPriorityId);
            builder.HasOne(p => p.TicketStatus).WithMany(c => c.Tickets).HasForeignKey(p => p.TicketStatusId);
            builder.HasOne(p => p.ReferedUser).WithMany(c => c.ReferredTickets).HasForeignKey(p => p.ReferedUserId);
            builder.HasOne(p => p.Creator).WithMany(c => c.CreatedTickets).HasForeignKey(p => p.CreatedBy);
            //builder.HasOne(p => p.Modifier).WithMany(c => c.ModifiedTickets).HasForeignKey(p => p.ModifierId);
        }
    }
}
