using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class XRF : BaseEntity<long>
    {
        public bool IsNeedGrind { get; set; }
        public bool IsNeedIOL { get; set; }
        public string Type { get; set; }
    }
}