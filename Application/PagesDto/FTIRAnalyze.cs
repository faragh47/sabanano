using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;

namespace CleanArchitecture.Application.PagesDto;
    public record FTIRAnalyzeDto:BaseRecordDto<FTIRAnalyzeDto,FTIR,long>
    {
        public string Name { get; set; }
        public string SampleState { get; set; }
        public string SampleType { get; set; }
        public string SampleComposition { get; set; }
    }
