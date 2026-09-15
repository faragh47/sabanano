using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Device
{
    public class AnalyzerDeviceSample : BaseEntity<int>
    {
        public int SampleCategoryId { get; set; }
        public int AnalyerDeviceId { get; set; }
        public AnalyzerDevice AnalyzerDevice { get; set; }
        public SampleCategory SampleCategory { get; set; }
    }
}
