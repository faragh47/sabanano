using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities.Device
{
	public class AnalyzeDeviceInput:BaseEntity<int>
	{
        [Required]
        public string Title { get; set; }
        public int AnalyzeDeviceId { get; set; }
        public bool? isList { get; set; }
		public string TextBox { get; set; }
		public bool? IsCheckbox { get; set; }
        public AnalyzerDevice AnalyzerDevice { get; set; }
    }
}

