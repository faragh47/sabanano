using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class AddressDto : BaseDto<AddressDto, Address,long>
{
    public string FullAddress { get; set; }
    public string PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}


public class AddressBriefDto : BaseDto<AddressBriefDto, Address, long>
{
    public string Address { get; set; }
    public string Title { get; set; }
    public string PhoneNumber { get; set; }
    public string PostalCode { get; set; }
    public string Description { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
