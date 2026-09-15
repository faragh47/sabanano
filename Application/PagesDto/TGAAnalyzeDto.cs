using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record TGAAnalyzeDto : BaseRecordDto<TGAAnalyzeDto, TGA, long>
{
    public double? TempertureStart { get; set; }
    public double TempertureEnd { get; set; }
    public double Rate { get; set; }
    public string Environment { get; set; }
}