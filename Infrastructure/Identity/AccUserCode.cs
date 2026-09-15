using System;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class AccUserCode: BaseAuditableEntity<long>
    {
        public string Code { get; set; }
        public long UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

