using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Common;

namespace Entities.DatabaseModels.TicketingModels
{
    public class TicketUserResponse : BaseAuditableEntity<long>
    {
        public long TicketId { get; set; }
        public long? PeopleId { get; set; }
        public string Description { get; set; }
        public long? InReplyToResponseId { get; set; }
        public Ticket Ticket { get; set; }
        [ForeignKey("InReplyToResponseId")]
        public TicketUserResponse InReplyToUserResponse { get; set; }
        public ICollection<TicketUserResponse> Children { get; set; }
        public ICollection<TicketAttachment> TicketAttachments { get; set; }

    }

    public class TicketUserResponseConfiguration : IEntityTypeConfiguration<TicketUserResponse>
    {
        public void Configure(EntityTypeBuilder<TicketUserResponse> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(2000);
            builder.Property(p => p.InReplyToResponseId).IsRequired(false);

            builder.HasOne(p => p.Ticket).WithMany(c => c.TicketUserResponses).HasForeignKey(p => p.TicketId);
            builder.HasOne(p => p.InReplyToUserResponse).WithMany(c => c.Children).HasForeignKey(c => c.InReplyToResponseId);
        }
    }
}
