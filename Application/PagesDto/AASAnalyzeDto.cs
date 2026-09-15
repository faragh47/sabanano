using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record AASAnalyzeDto : BaseRecordDto<AASAnalyzeDto, AAS, long>
{
    public string Element { get; set; }
    public bool IsNeedDigesting { get; set; }
    public string? Description { get; set; }
}