using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class FTIR : BaseEntity<long>
    {
        public string SampleState { get; set; }
        public string SampleType { get; set; }
        public string? SampleComposition { get; set; }
    }
}