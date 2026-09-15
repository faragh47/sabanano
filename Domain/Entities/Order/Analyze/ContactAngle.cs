using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class ContactAngle : BaseEntity<long>
    {
        public string Breed { get; set; }
        public bool IsNeedDigesting { get; set; }
        public string? Description { get; set; }
    }
}