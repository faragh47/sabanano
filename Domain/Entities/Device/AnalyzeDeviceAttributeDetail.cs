using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities.Device
{
	public class AnalyzeDeviceAttributeDetail:BaseEntity<int>
	{
        public AnalyzeDeviceAttribute AnalyzeDeviceAttribute { get; set; }
        public int AnalyzeAttributeId { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}

