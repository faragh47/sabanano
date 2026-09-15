using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Grants;
using CleanArchitecture.Domain.Entities.Order;

namespace CleanArchitecture.Application.TodoItems.Queries.Grants;

public class GrantListDto : BaseDto<GrantListDto, Grant,long>
{
    public string NationalCode { get; set; }
}


public class GrantBriefDto : BaseDto<GrantBriefDto, Grant, long>
{
    public string NationalCode { get; set; }
}
