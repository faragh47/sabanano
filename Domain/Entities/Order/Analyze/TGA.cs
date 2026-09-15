using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class TGA : BaseEntity<long>
    {
        public double? TempertureStart { get; set; }
        public double TempertureEnd { get; set; }
        public double Rate { get; set; }
        public string Environment { get; set; }
    }
}