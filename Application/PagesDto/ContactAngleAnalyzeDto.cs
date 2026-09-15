using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record ContactAngleAnalyzeDto : BaseRecordDto<ContactAngleAnalyzeDto, ContactAngle, long>
{
    public string Breed { get; set; }
    public bool IsNeedDigesting { get; set; }
    public string? Description { get; set; }
}