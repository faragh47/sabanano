using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record EDXAnalyzeDto : BaseRecordDto<EDXAnalyzeDto, EDX, long>
{
    public string Element { get; set; }
}