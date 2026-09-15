using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;
    public record BETAnalyzeDto:BaseRecordDto<BETAnalyzeDto,BET,long>
    {
        public string Name { get; set; }
        public double DegassingTemperature { get; set; }
        public double Time { get; set; }
    }
