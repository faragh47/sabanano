using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;

public record GCMSnalyzeDto : BaseRecordDto<GCMSnalyzeDto, GCMS, long>
{
    public string? Solvent { get; set; }
    public string? SampleNature { get; set; }
    public string? Composition { get; set; }
}

