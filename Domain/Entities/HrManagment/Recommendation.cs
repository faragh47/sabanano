namespace CleanArchitecture.Domain.Entities.HrManagment;

public class Recommendation : BaseAuditableEntity<int>
{
    public string Description { get; set; }
}