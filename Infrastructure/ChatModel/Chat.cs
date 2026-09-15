using System;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Identity;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.ChatModel
{
    public class Chat:BaseAuditableEntity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long ContextId { get; set; }
        public long ReferedUserId { get; set; }
        [ForeignKey("ReferedUserId")]
        public ApplicationUser ReferedUser { get; set; }
        [ForeignKey("CreatedBy")]
        public ApplicationUser Creator { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(x => x.ReferedUserId).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.HasIndex(x => new { x.CreatedBy, x.ReferedUserId }).IsUnique();
            builder.HasOne(p => p.ReferedUser).WithMany(c => c.ReferedChats).HasForeignKey(p => p.ReferedUserId);
            builder.HasOne(p => p.Creator).WithMany(c => c.CreatedByChats).HasForeignKey(p => p.CreatedBy);
            //builder.HasOne(p => p.Modifier).WithMany(c => c.ModifiedTickets).HasForeignKey(p => p.ModifierId);
        }
    }
}

