using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class PeopleAddressBriefDto : BaseDto<PeopleAddressBriefDto, PeopleAddress, long>
{
    public string Address { get; set; }
    public string Title { get; set; }
    public string PhoneNumber { get; set; }
    public string PostalCode { get; set; }
    public string Description { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int Floor { get; set; }
    public int Unit { get; set; }
    public int Number { get; set; }
    public long Id { get; set; }

    public override void CustomMappings(IMappingExpression<PeopleAddress, PeopleAddressBriefDto> mappingExpression)
    {
        mappingExpression.ForMember(dest => dest.Address, conf => conf.MapFrom(src => src.Address.FullAddress));
        mappingExpression.ForMember(dest => dest.Floor, conf => conf.MapFrom(src => src.Address.Floor));
        mappingExpression.ForMember(dest => dest.Unit, conf => conf.MapFrom(src => src.Address.Unit));
        mappingExpression.ForMember(dest => dest.Number, conf => conf.MapFrom(src => src.Address.Number));
        mappingExpression.ForMember(dest => dest.Address, conf => conf.MapFrom(src => src.Address.FullAddress));
        mappingExpression.ForMember(dest => dest.PhoneNumber, conf => conf.MapFrom(src => src.Address.PhoneNumber));
        mappingExpression.ForMember(dest => dest.PostalCode, conf => conf.MapFrom(src => src.Address.PostalCode));
        mappingExpression.ForMember(dest => dest.Latitude, conf => conf.MapFrom(src => src.Address.Latitude));
        mappingExpression.ForMember(dest => dest.Longitude, conf => conf.MapFrom(src => src.Address.Longitude));
        mappingExpression.ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Address.Id));
    }
}
