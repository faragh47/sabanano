
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Common;

namespace Entities.DatabaseModels.TicketingModels
{
    public class TicketAttachment : BaseAuditableEntity<long>
    {
        public long TicketResponseId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }

        public TicketUserResponse TicketUserResponse { get; set; }
    }

    public class TicketAttachmentConfiguration : IEntityTypeConfiguration<TicketAttachment>
    {
        public void Configure(EntityTypeBuilder<TicketAttachment> builder)
        {
            builder.Property(c => c.FileName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.FileExt).IsRequired().HasMaxLength(6);

            builder.HasOne(c => c.TicketUserResponse).WithMany(p => p.TicketAttachments).HasForeignKey(p => p.TicketResponseId);
        }
    }
}
