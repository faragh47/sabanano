using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class BET:BaseEntity<long>
    {
        public double DegassingTemperature { get; set; }
        public double Time { get; set; }
    }
}
