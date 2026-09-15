using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CleanArchitecture.Domain.Common;

namespace Entities.DatabaseModels.TicketingModels
{
    public class TicketStatusHistory : BaseAuditableEntity<long>
    {
        public long TicketId { get; set; }
        public int StatusId { get; set; }
        public Ticket Ticket { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }

    public class TicketStatusChangeConfiguration : IEntityTypeConfiguration<TicketStatusHistory>
    {
        public void Configure(EntityTypeBuilder<TicketStatusHistory> builder)
        {
            builder.HasOne(p => p.Ticket).WithMany(c => c.TicketStatusHistories).HasForeignKey(p => p.TicketId);
            builder.HasOne(p => p.TicketStatus).WithMany(c => c.TicketStatusHistories).HasForeignKey(p => p.StatusId);
        }
    }
}
