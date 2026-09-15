using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record UVAnalyzeDto : BaseRecordDto<UVAnalyzeDto, UV, long>
{
    public string? Type { get; set; }
    public string? Solvent { get; set; }
    public double? WaveLengthStart { get; set; }
    public double? WaveLengthEnd { get; set; }
    public string Spectrum { get; set; }
}