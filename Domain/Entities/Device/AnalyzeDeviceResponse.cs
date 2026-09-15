using System;
namespace CleanArchitecture.Domain.Entities.Device
{
	public class AnalyzeDeviceResponse:BaseEntity<int>
	{
		public int AnalyzeDeviceId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public long? ImageId { get; set; }
        public Image? Image { get; set; }
        public AnalyzerDevice AnalyzerDevice { get; set; }

    }
}

