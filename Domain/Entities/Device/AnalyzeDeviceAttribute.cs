using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Device
{
    public class AnalyzeDeviceAttribute : BaseEntity<int>
    {
        public int AnalyzeDeviceId { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public AnalyzerDevice AnalyzerDevice { get; set; }
        public ICollection<AnalyzeDeviceAttributeDetail> Details { get; set; }
    }
}
