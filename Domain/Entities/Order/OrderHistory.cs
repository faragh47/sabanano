using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Domain.Entities.Orders
{
    public class OrderHistory : BaseAuditableEntity<long>
    {
        public string TechnicalComment { get; set; }
        public long OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        #region Relationships

        public Order Order { get; set; }

        #endregion
    }
}