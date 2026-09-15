using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class CompanyListDto : BaseDto<CompanyListDto, Company,long>
{
    public string NationalCode { get; set; }
    public string RegisterCode { get; set; }
    public string MobileNumber { get; set; }
    public string EconomicNumber { get; set; }
}


public class CompanyBriefDto : BaseDto<CompanyBriefDto, Company, long>
{
    public string NationalCode { get; set; }
    public string RegisterCode { get; set; }
    public string MobileNumber { get; set; }
    public string EconomicNumber { get; set; }
}
