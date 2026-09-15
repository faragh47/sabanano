using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

public class AnalyzerDeviceListDto : BaseDto<AnalyzerDeviceListDto, AnalyzerDevice, int>
{
    public string RegisterForm { get; set; }
    public string Name { get; set; }
    public string PersianName { get; set; }
    public string FullName { get; set; }
    public string Country { get; set; }
    public string CompanyName { get; set; }
    public string SpectroscopyRange { get; set; }
    public string Usage { get; set; }
    public string Model { get; set; }
    public string MaintenanceCondition { get; set; }
    public decimal Price { get; set; }
    public decimal PriceWithDiscount { get; set; }
    public int? DiscountPercent { get; set; }
    public string DescriptionForReadyAnalyze { get; set; }
    public string RequirementSample { get; set; }
    public string Sample { get; set; }
    public string MainImage { get; set; }
    public List<AnalyzeDeviceServiceListDto> Services { get; set; }
    public List<AnalyzeDeviceAttributeListDto> Attributes { get; set; }
    public List<AnalyzeDeviceInputListDto> Inputs { get; set; }
    public List<AnalyzeDeviceResponseListDto> Responses { get; set; }

    public override void CustomMappings(IMappingExpression<AnalyzerDevice, AnalyzerDeviceListDto> mapping)
    {
        mapping.ForMember(dest => dest.Sample,
            conf => conf.MapFrom(src => string.Join("، ", src.Samples.Select(x => x.SampleCategory.Title))));
        mapping.ForMember(dest => dest.PriceWithDiscount,
            conf => conf.MapFrom(src => CalculateDiscount(src.Price, src.DiscountPercent)));
        mapping.ForMember(dest => dest.MainImage, conf => conf.MapFrom(src => "../img/" + src.Name + ".png"));
    }

    private static decimal CalculateDiscount(decimal price, int? discountPercent)
    {
        if (discountPercent is null || discountPercent == 0)
            return price;
        else
            return (price * Convert.ToInt32(discountPercent)) / 100;
    }
}


public class AnalyzerDeviceBriefListDto : BaseDto<AnalyzerDeviceBriefListDto, AnalyzerDevice, int>
{
    public string Name { get; set; }
    public string MainImage { get; set; }

    public override void CustomMappings(IMappingExpression<AnalyzerDevice, AnalyzerDeviceBriefListDto> mapping)
    {
        mapping.ForMember(dest => dest.MainImage,
            conf => conf.MapFrom(src => "../img/" + src.Name + ".png"));
    }
}