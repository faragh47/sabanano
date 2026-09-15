using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;




public class AnalyzeDeviceAttributeListDto : BaseDto<AnalyzeDeviceAttributeListDto, AnalyzeDeviceAttribute,int>
{
    public int AnalyzeDeviceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<AnalyzeDeviceAttributeDetailListDto> Details { get; set; }
}


public class AnalyzeDeviceAttributeBriefListDto : BaseDto<AnalyzeDeviceAttributeBriefListDto, AnalyzeDeviceAttribute, int>
{
    public int SampleCategoryId { get; set; }
    public int AnalyerDeviceId { get; set; }
}
