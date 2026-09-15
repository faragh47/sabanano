using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Domain.Entities.Orders
{
    public class OrderAnalyze : BaseEntity<long>
    {
        [Required] public string TrackingCode { get; set; }
        public string? Description { get; set; }
        public string? AdditionalDescription { get; set; }
        public bool IsSensitiveToLight { get; set; }
        public bool IsSensitiveToHumidity { get; set; }
        public bool HasNotAnyCondition { get; set; }
        
        public double? SpeceficTemperture { get; set; }
        public double? SpeceficAtmosphere { get; set; }
        public long OrderId { get; set; }
        public long? SafetyId { get; set; }
        public long? AnalyzeModelId { get; set; }
        public int AnalyzeDeviceId { get; set; }
        public string Name { get; set; }

        #region Relationships

        public Order Order { get; set; }
        public Safety Safety { get; set; }
        public AnalyzerDevice AnalyzerDevice { get; set; }

        #endregion
    }
}