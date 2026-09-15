using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class TEM : BaseEntity<long>
    {
        public double? Zoom { get; set; }
        public double? Size { get; set; }
        public string? Description { get; set; }
    }
}