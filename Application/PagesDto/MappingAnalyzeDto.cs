using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record MappingAnalyzeDto : BaseRecordDto<MappingAnalyzeDto, Mapping, long>
{
    public string Element { get; set; }
}