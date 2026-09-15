using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

public class ComplaintDto : BaseDto<ComplaintDto, Complaint, int>
{
    public string? TrackingCodes { get; set; }
    public string? OrderDescription { get; set; }
    public long? ImageId { get; set; }
    public string Description { get; set; }
    public string Recomendation { get; set; }
}