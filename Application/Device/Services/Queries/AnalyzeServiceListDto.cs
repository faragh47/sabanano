using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;




public class AnalyzeDeviceServiceListDto : BaseDto<AnalyzeDeviceServiceListDto, AnalyzeDeviceService,int>
{
    public int AnalyzeDeviceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
}


public class AnalyzeDeviceServiceBriefListDto : BaseDto<AnalyzeDeviceServiceBriefListDto, AnalyzeDeviceService, int>
{
    public int AnalyzeDeviceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
}
