using System;
namespace CleanArchitecture.Domain.Entities
{
    public class PersonMobileNumber : BaseAuditableEntity<long>
    {
        public string MobileNumber { get; set; }
        public bool? IsDefault { get; set; }
        //  public long FPersonId { get; set; }
        public long PersonId { get; set; }
        //public CreIndividual Individual { get; set; }
        public Person Person { get; set; }
    }
}

