using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record XRDAnalyzeDto : BaseRecordDto<XRDAnalyzeDto, XRD, long>
{
    public double AngleStart { get; set; }
    public double AngleEnd { get; set; }
    public string? Composition { get; set; }
    public bool IsNeedGrind { get; set; }
    public string Type { get; set; }
}