using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Domain.Entities.FinancialAggregate
{
    public class Discount : BaseAuditableEntity<long>
    {
        public DateTime? ExpirationDate { get; set; }
        public int? Count { get; set; }
        public int DistcountTypeId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal? Amount { get; set; }
        public int? Percent { get; set; }
        public decimal? Max { get; set; }
        public string LatinTitle { get; set; }
        public string Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public ICollection<PersonDiscount> PersonDiscounts { get; set; }
        public ICollection<DiscountIncrease> Increases { get; set; }
        public ICollection<DiscountUsed> Useds { get; set; }
    }

}
