using CleanArchitecture.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Order.Analyze
{
    public class UV : BaseEntity<long>
    {
        public string? Type { get; set; }
        public string? Solvent { get; set; }
        public double? WaveLengthStart { get; set; }
        public double? WaveLengthEnd { get; set; }
        public string Spectrum { get; set; }
    }
}