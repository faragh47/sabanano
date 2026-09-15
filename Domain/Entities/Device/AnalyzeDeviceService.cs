using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Device
{
    public class AnalyzeDeviceService  :BaseEntity<int>
    {
        public int AnalyzeDeviceId { get; set; }
        [MaxLength(450)]
        public string Title { get; set; }
        [MaxLength(1200)]
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public AnalyzerDevice AnalyzerDevice { get; set; }
    }
}
