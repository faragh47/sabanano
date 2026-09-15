using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;




public class AnalyzerDeviceSampleListDto : BaseDto<AnalyzerDeviceSampleListDto, AnalyzerDeviceSample,int>
{
    public string SampleCategoryTitle { get; set; }
    public int AnalyerDeviceId { get; set; }
}


public class AnalyzerDeviceSampleBriefListDto : BaseDto<AnalyzerDeviceSampleBriefListDto, AnalyzerDeviceSample, int>
{
    public int SampleCategoryId { get; set; }
    public int AnalyerDeviceId { get; set; }
}
