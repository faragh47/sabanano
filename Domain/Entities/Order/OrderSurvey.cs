using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Domain.Entities.Orders
{
    public class OrderSurvey : BaseAuditableEntity<long>
    {
        public string Comment { get; set; }
        public SurveyScore Score { get; set; }
        public long OrderId { get; set; }
        #region Relationships

        public Order Order { get; set; }

        #endregion
    }
}