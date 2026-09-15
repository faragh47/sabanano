using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class XRD : BaseEntity<long>
    {
        public double AngleStart { get; set; }
        public double AngleEnd { get; set; }
        public string? Composition { get; set; }
        public bool IsNeedGrind { get; set; }
        public string Type { get; set; }
    }
}