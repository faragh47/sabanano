using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class GCMS : BaseEntity<long>
    {
        public string SampleNature { get; set; }
        public string? Solvent { get; set; }
        public string? Composition { get; set; }
    }
}