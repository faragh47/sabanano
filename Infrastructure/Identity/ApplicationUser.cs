using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.ChatModel;
using Entities.DatabaseModels.NotificationModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<long>,IEntity<long>
    {
        public ApplicationUser()
        {
            IsActive = true;
            EmailConfirmed = true;
        }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public long CreatorId { get; set; }
        public long ModifierId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public long PersonId { get; set; }
        public Person Person { get; set; }
        public ICollection<Ticket> ReferredTickets { get; set; }
        public ICollection<Ticket> CreatedTickets { get; set; }
        public ICollection<TicketReferHistory> TicketRefersCreated { get; set; }
        public ICollection<TicketReferHistory> TicketRefersFrom { get; set; }
        public ICollection<TicketReferHistory> TicketRefersTo { get; set; }
        public ICollection<Notification> NotificationCreator { get; set; }
        public ICollection<Notification> NotificationUser { get; set; }
        public ICollection<Chat> ReferedChats { get; set; }
        public ICollection<Chat> CreatedByChats { get; set; }
        public ICollection<AccUserCode> Codes { get; set; }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(x => x.Person).WithMany().HasForeignKey(x=>x.PersonId);
        }
    }
}