using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;




public class FinancialDetailListDto : BaseDto<FinancialDetailListDto, FinancialDetail,long>
{
    public long FinancialId { get; set; }
    public decimal Price { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}


public class FinancialDetailBriefListDto : BaseDto<FinancialDetailBriefListDto, FinancialDetail, long>
{
    public long FinancialId { get; set; }
    public decimal Price { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
