using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class CustomerListDto : BaseDto<CustomerListDto, Customer,long>
{
    public string CustomerCode { get; set; }
    public PeopleBriefDto Person { get; set; }
}


public class CustomerBriefDto : BaseDto<CustomerBriefDto, Customer, long>
{
    public string CustomerCode { get; set; }
}

public class CustomerInfo
{
    public string FullName { get; set; }
    public string NationalCode { get; set; }
    public string MobileNumber { get; set; }
    public string OrganizationName { get; set; }
    public string EmailAddress { get; set; }
    public string Address { get; set; }
}