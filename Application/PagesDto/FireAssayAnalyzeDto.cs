using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record FireAssayAnalyzeDto : BaseRecordDto<FireAssayAnalyzeDto, FireAssay, long>
{
    public string Element { get; set; }
    public string Description { get; set; }
}