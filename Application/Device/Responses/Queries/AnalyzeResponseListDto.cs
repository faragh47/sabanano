using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;




public class AnalyzeDeviceResponseListDto : BaseDto<AnalyzeDeviceResponseListDto, AnalyzeDeviceResponse,int>
{
    public int AnalyzeDeviceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public long? ImageId { get; set; }
    public override void CustomMappings(IMappingExpression<AnalyzeDeviceResponse, AnalyzeDeviceResponseListDto> mapping)
    {
        mapping.ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => "../img/Device/" + src.Image.FileName));
    }
}


public class AnalyzeDeviceResponseBriefListDto : BaseDto<AnalyzeDeviceResponseBriefListDto, AnalyzeDeviceResponse, int>
{
    public int AnalyzeDeviceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
}
