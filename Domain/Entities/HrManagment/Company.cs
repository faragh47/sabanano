using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities.HrManagment.Companies;

public class Company : BaseAuditableEntity<long>
{
    [MaxLength(50)]
    public string NationalCode { get; set; }
    [MaxLength(50)]
    public string RegisterCode { get; set; }
    [MaxLength(50)]
    public string MobileNumber { get; set; }
    [MaxLength(50)]
    public string EconomicNumber { get; set; }
    public ICollection<Orders.Order> Orders { get; set; }
}