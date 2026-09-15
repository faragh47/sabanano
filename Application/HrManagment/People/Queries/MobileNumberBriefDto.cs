using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class MobileNumberBriefDto : BaseDto<MobileNumberBriefDto, PersonMobileNumber,long>
{
    public string MobileNumber { get; set; }
}
