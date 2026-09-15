namespace CleanArchitecture.Domain.Entities.Grants;

public class Grant:BaseEntity<long>
{
    public string NationalCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? UniversityName { get; set; }
    public string? TelNumber { get; set; }
    public ICollection<Orders.Order> Orders { get; set; }
}