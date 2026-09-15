using System;
using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.Domain.Entities.HrManagment
{
    public class PeopleAddress : BaseAuditableEntity<long>
    {
        [AllowNull]
        public string Title { get; set; }
        public long AddressId { get; set; }
        public long PersonId { get; set; }
        public bool IsDefault { get; set; }
        public Person Person { get; set; }
        public Address Address { get; set; }
    }
}

