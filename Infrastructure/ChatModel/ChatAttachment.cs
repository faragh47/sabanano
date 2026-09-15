
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.ChatModel;

namespace CleanArchitecture.Infrastructure.ChatModel
{
    public class ChatAttachment : BaseAuditableEntity<long>
    {
        public long MessageId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }
        public Message Message { get; set; }
    }

    public class ChatAttachmentConfiguration : IEntityTypeConfiguration<ChatAttachment>
    {
        public void Configure(EntityTypeBuilder<ChatAttachment> builder)
        {
            builder.Property(c => c.FileName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.FileExt).IsRequired().HasMaxLength(6);
            builder.HasOne(c => c.Message).WithMany(p => p.Attachments).HasForeignKey(p => p.MessageId);
        }
    }
}
