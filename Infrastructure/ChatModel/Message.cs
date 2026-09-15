using System;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.ChatModel
{
    public class Message: BaseAuditableEntity<long>
    {
        public long ChatId { get; set; }
        public string Description { get; set; }
        public long? InReplyToResponseId { get; set; }
        public bool IsSeen { get; set; }
        [ForeignKey("InReplyToResponseId")]
        public Message InReplyToUserResponse { get; set; }
        public Chat Chat { get; set; }
        public ICollection<Message> Children { get; set; }
        public ICollection<ChatAttachment> Attachments { get; set; }
    }

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(2000);
            builder.Property(p => p.InReplyToResponseId).IsRequired(false);
            builder.HasOne(p => p.Chat).WithMany(c => c.Messages).HasForeignKey(p => p.ChatId);
            builder.HasOne(p => p.InReplyToUserResponse).WithMany(c => c.Children).HasForeignKey(c => c.InReplyToResponseId);
        }
    }
}

