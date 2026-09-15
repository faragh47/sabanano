using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;




public class AnalyzeDeviceAttributeDetailListDto : BaseDto<AnalyzeDeviceAttributeDetailListDto, AnalyzeDeviceAttributeDetail,int>
{
    public int AnalyzeAttributeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}


public class AnalyzeDeviceAttributeDetailBriefListDto : BaseDto<AnalyzeDeviceAttributeDetailBriefListDto, AnalyzeDeviceAttributeDetail, int>
{
    public int AnalyzeAttributeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
