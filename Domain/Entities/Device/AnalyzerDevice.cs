using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Orders;

namespace CleanArchitecture.Domain.Entities.Device
{
    public class AnalyzerDevice : BaseAuditableEntity<int>
    {
        [MaxLength(450)]
        public string RegisterForm { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(100)]
        public string Code { get; set; }    
        [MaxLength(100)]
        public string DocumentCode { get; set; }
        [MaxLength(100)]
        public string PersianName { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
        [MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string SpectroscopyRange { get; set; }
        [MaxLength(450)]
        public string Usage { get; set; }
        [MaxLength(100)]
        public string Model { get; set; }
        [MaxLength(450)]
        public string MaintenanceCondition { get; set; }
        [MaxLength(450)]
        public string RequirementSample { get; set; }
        public decimal Price { get; set; }
        public int? DiscountPercent { get; set; }
        [MaxLength(1200)]
        public string DescriptionForReadyAnalyze { get; set; }
        public ICollection<AnalyzerDeviceSample> Samples { get; set; }
        public ICollection<AnalyzeDeviceService> Services { get; set; }
        public ICollection<AnalyzeDeviceAttribute> Attributes { get; set; }
        public ICollection<AnalyzeDeviceInput> Inputs { get; set; }
        public ICollection<AnalyzeDeviceResponse> Responses { get; set; }
        public ICollection<OrderAnalyze> OrderAnalyzes { get; set; }
    }
}
