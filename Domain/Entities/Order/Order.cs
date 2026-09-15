using System;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.Grants;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using CleanArchitecture.Domain.Entities.Order;

namespace CleanArchitecture.Domain.Entities.Orders
{
    public class Order : BaseAuditableEntity<long>
    {
        public long? GrantId { get; set; }
        public long? CompanyId { get; set; }
        public long? FinancialId { get; set; }
        public bool IsRequireToReturnSample { get; set; }
        public bool IsRequireHeader { get; set; }
        public double? ProcessingTime { get; set; }
        public long? ImageId { get; set; }
        public long? PersonId { get; set; }
        public Company? Company { get; set; }
        public Image? Image { get; set; }
        public Financial? Financial { get; set; }
        [Required] public string TrackingCode { get; set; } = null!;
        public string OrderNumber { get; set; } = null!;
        public string Description { get; set; } = null!;

        public Grant? Grant { get; set; }
        public OrderStatus OrderStatus { get; set; } = null!;
        public Person? Person { get; set; }

        #region Relationships

        public ICollection<OrderAnalyze> OrderAnalyzes { get; set; }
        public ICollection<OrderHistory> Histories { get; set; }
        public ICollection<OrderSurvey> Surveys { get; set; }

        #endregion
    }
}