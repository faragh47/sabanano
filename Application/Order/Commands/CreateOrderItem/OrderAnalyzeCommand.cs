using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Domain.Entities.Orders;
using FluentValidation.Validators;
using MediatR;

namespace CleanArchitecture.Application.Orders.Commands.Create;

public record OrderAnalyzeCommand : BaseRecordDto<OrderAnalyzeCommand, OrderAnalyze, long>, IRequest<long>
{
    public string TrackingCode { get; set; }
    public string Description { get; set; }
    public string AdditionalDescription { get; set; }
    public bool IsSensitiveToLight { get; set; }
    public bool IsSensitiveToHumidity { get; set; }
    public double? SpeceficTemperture { get; set; }
    public double? SpeceficAtmosphere { get; set; }
    public bool HasNotAnyCondition { get; set; }
    public bool HasNoSafety { get; set; }
    public bool IsExplosive { get; set; }
    public bool IsPoisonous { get; set; }
    public bool IsEscapable { get; set; }
    public bool IsFlammable { get; set; }
    public bool IsBadForBreathing { get; set; }
    public bool IsAdsorbBySkin { get; set; }
    public bool IsNanoSize { get; set; }
    public bool IsSickness { get; set; }
    public long OrderId { get; set; }
    public long? SafetyId { get; set; }
    public long AnalyzeDeviceId { get; set; }
    public BETAnalyzeDto Bet { get; set; }
    public FTIRAnalyzeDto Ftir { get; set; }
    public AASAnalyzeDto AAS { get; set; }
    public SEMAnalyzeDto SEM { get; set; }
    public TEMAnalyzeDto TEM { get; set; }
    public TGAAnalyzeDto TGA { get; set; }
    public UVAnalyzeDto UV { get; set; }
    public AFMAnalyzeDto AFM { get; set; }
    public EDXAnalyzeDto EDX { get; set; }
    public XRDAnalyzeDto XRD { get; set; }
    public XRFAnalyzeDto XRF { get; set; }
    public FireAssayAnalyzeDto FireAssay { get; set; }
    public ICPMSAnalyzeDto ICPMS { get; set; }
    public ICPOESAnalyzeDto ICPOES { get; set; }
    public MappingAnalyzeDto Mapping { get; set; }
    public GCMSnalyzeDto GCMS { get; set; }
    public ContactAngleAnalyzeDto ContactAngle { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public bool IsView { get; set; }
    public string Key { get; set; }

    public override void CustomMappings(IMappingExpression<OrderAnalyze, OrderAnalyzeCommand> mapping)
    {
        mapping.ForMember(dest => dest.HasNoSafety,
            conf => conf.MapFrom(src => src.Safety.HasNoSafety));
        mapping.ForMember(dest => dest.IsExplosive,
            conf => conf.MapFrom(src => src.Safety.IsExplosive));
        mapping.ForMember(dest => dest.IsPoisonous,
            conf => conf.MapFrom(src => src.Safety.IsPoisonous));
        mapping.ForMember(dest => dest.IsEscapable,
            conf => conf.MapFrom(src => src.Safety.IsEscapable));
        mapping.ForMember(dest => dest.IsFlammable,
            conf => conf.MapFrom(src => src.Safety.IsFlammable));
        mapping.ForMember(dest => dest.IsBadForBreathing,
            conf => conf.MapFrom(src => src.Safety.IsBadForBreathing));
        mapping.ForMember(dest => dest.IsAdsorbBySkin,
            conf => conf.MapFrom(src => src.Safety.IsAdsorbBySkin));
        mapping.ForMember(dest => dest.IsNanoSize,
            conf => conf.MapFrom(src => src.Safety.IsNanoSize));
        mapping.ForMember(dest => dest.IsSickness,
            conf => conf.MapFrom(src => src.Safety.IsSickness));
    }
}