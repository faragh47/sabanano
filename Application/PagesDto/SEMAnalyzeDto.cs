using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record SEMAnalyzeDto : BaseRecordDto<SEMAnalyzeDto, SEM, long>
{
    public double? Zoom { get; set; }
    public double? Size { get; set; }
    public string? Description { get; set; }
}