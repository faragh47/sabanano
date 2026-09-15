using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Orders;

namespace CleanArchitecture.Application.Orders;

public class OrderAnalyzeDto : BaseDto<OrderAnalyzeDto, OrderAnalyze, long>
{
    public string TrackingCode { get; set; }
    public string? Description { get; set; }
    public string? AdditionalDescription { get; set; }
    public bool IsRequireToReturnSample { get; set; }
    public bool IsRequireHeader { get; set; }
    public bool IsSensitiveToLight { get; set; }
    public bool IsSensitiveToHumidity { get; set; }
    public double? SpeceficTemperture { get; set; }
    public double? SpeceficAtmosphere { get; set; }
    public bool IsPoisonous { get; set; }
    public bool IsEscapable { get; set; }
    public bool IsFlammable { get; set; }
    public bool IsBadForBreathing { get; set; }
    public bool IsAdsorbBySkin { get; set; }
    public bool IsNanoSize { get; set; }
    public bool IsSickness { get; set; }
    public override void CustomMappings(IMappingExpression<OrderAnalyze, OrderAnalyzeDto> mapping)
    {
        mapping
            .ForMember(dest => dest.IsEscapable,conf => conf.MapFrom(src => src.Safety.IsEscapable))
            .ForMember(dest => dest.IsFlammable,conf => conf.MapFrom(src => src.Safety.IsFlammable))
            .ForMember(dest => dest.IsBadForBreathing,conf => conf.MapFrom(src => src.Safety.IsBadForBreathing))
            .ForMember(dest => dest.IsAdsorbBySkin,conf => conf.MapFrom(src => src.Safety.IsAdsorbBySkin))
            .ForMember(dest => dest.IsNanoSize,conf => conf.MapFrom(src => src.Safety.IsNanoSize))
            .ForMember(dest => dest.IsSickness,conf => conf.MapFrom(src => src.Safety.IsSickness));
    }
}

public class OrderAnalyzeBriefDto : BaseDto<OrderAnalyzeBriefDto, OrderAnalyze, long>
{
    public string Name { get; set; }
    public string TrackingCode { get; set; }
}