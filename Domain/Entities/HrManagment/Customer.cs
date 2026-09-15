using System;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.Entities.HrManagment
{
    public class Customer : BaseAuditableEntity<long>
    {
        public long PersonId { get; set; }
        public long AccountId { get; set; }
        public string CustomerCode { get; set; }
        public Person Person { get; set; }
        public Account Account { get; set; }
    }
}

