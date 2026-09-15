namespace CleanArchitecture.Domain.Entities.HrManagment;

public class Complaint : BaseAuditableEntity<int>
{
    public string? TrackingCodes { get; set; }
    public string? OrderDescription { get; set; }
    public long? ImageId { get; set; }
    public string Description { get; set; }
    public string Recomendation { get; set; }
    public Image Image { get; set; }
}   