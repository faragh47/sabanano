using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class PeopleBriefDto : BaseDto<PeopleBriefDto, Person, long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public override void CustomMappings(IMappingExpression<Person, PeopleBriefDto> mappingExpression)
    {
        mappingExpression.ForMember(dest => dest.LastName, conf => conf.MapFrom(src => src.FirstName));
        mappingExpression.ForMember(dest => dest.FirstName, conf => conf.MapFrom(src => src.LastName));
        mappingExpression.ForMember(dest => dest.Mobile,
            conf => conf.MapFrom(src => GetMobileNumber(src.MobileNumbers)));
        mappingExpression.ForMember(dest => dest.Address,
            conf => conf.MapFrom(src => GetAddress(src.PeopleAddresses)));
        mappingExpression.ForMember(dest => dest.CompanyName, conf => conf.MapFrom(src => src.CompanyName));
    }

    public static string GetMobileNumber(ICollection<PersonMobileNumber> mobileNumbers) =>
        mobileNumbers is { Count: > 0 } ? mobileNumbers.FirstOrDefault()?.MobileNumber : string.Empty;

    public static string GetAddress(ICollection<PeopleAddress> addresses) =>
        addresses is { Count: > 0 } ? addresses.FirstOrDefault().Address.FullAddress : string.Empty;
}