using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;


namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class DiscountUsedListDto : BaseDto<DiscountUsedListDto, DiscountUsed,long>
{
    public long DiscountId { get; set; }
    public long PersonId { get; set; }
    //public ServceTypeBriefListDto Deliveryman{ get; set; }
}


public class DiscountUsedBriefListDto : BaseDto<DiscountUsedBriefListDto, DiscountUsed, long>
{
    public long DiscountId { get; set; }
    public long PersonId { get; set; }
}
