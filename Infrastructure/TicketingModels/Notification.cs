using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DatabaseModels.NotificationModels
{
    public class Notification : BaseAuditableEntity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long? UserId { get; set; }
        public long? PersonId { get; set; }
        public int? ContextId { get; set; }
        public bool SendSMS { get; set; }
        public bool SendInApp { get; set; }
        public DateTime SendDate { get; set; }
        public bool Seen { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [ForeignKey("CreatorId")]
        public new ApplicationUser Creator { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }

    public class NtfNotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);

            //builder.HasOne(p => p.Person).WithMany(c => c.Notification).HasForeignKey(c => c.FPersonId);
            builder.HasOne(p => p.Creator).WithMany(c => c.NotificationCreator).HasForeignKey(c => c.CreatedBy);
            builder.HasOne(p => p.User).WithMany(c => c.NotificationUser).HasForeignKey(c => c.UserId);
        }
    }
}
