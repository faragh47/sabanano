using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record XRFAnalyzeDto : BaseRecordDto<XRFAnalyzeDto, XRF, long>
{
    public bool IsNeedGrind { get; set; }
    public bool IsNeedIOL { get; set; }
    public string Type { get; set; }
}