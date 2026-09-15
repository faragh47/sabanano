using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;


namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class DiscountIncreaseListDto : BaseDto<DiscountIncreaseListDto, DiscountIncrease,long>
{
    public long DistcountId { get; set; }
    public int Percent { get; set; }
    //public ServceTypeBriefListDto Deliveryman{ get; set; }
}


public class DiscountIncreaseBriefListDto : BaseDto<DiscountIncreaseBriefListDto, DiscountIncrease, long>
{
    public long DistcountId { get; set; }
    public int Percent { get; set; }
}
