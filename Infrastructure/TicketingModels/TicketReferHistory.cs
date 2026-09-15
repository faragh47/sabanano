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
    public class TicketReferHistory : BaseAuditableEntity<long>
    {
        public long TicketId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }

        public Ticket Ticket { get; set; }

        [ForeignKey("CreatorId")]
        public new ApplicationUser Creator { get; set; }
        [ForeignKey("FromUserId")]
        public ApplicationUser FromUser { get; set; }
        [ForeignKey("ToUserId")]
        public ApplicationUser ToUser { get; set; }

    }

    public class TicketReferHistoryConfiguration : IEntityTypeConfiguration<TicketReferHistory>
    {
        public void Configure(EntityTypeBuilder<TicketReferHistory> builder)
        {
            builder.HasOne(p => p.Ticket).WithMany(c => c.TicketRefers).HasForeignKey(p => p.TicketId);
            builder.HasOne(p => p.Creator).WithMany(c => c.TicketRefersCreated).HasForeignKey(p => p.CreatedBy);
            builder.HasOne(p => p.FromUser).WithMany(c => c.TicketRefersFrom).HasForeignKey(p => p.FromUserId);
            builder.HasOne(p => p.ToUser).WithMany(c => c.TicketRefersTo).HasForeignKey(p => p.ToUserId);
        }
    }
}
