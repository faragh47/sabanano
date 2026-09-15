using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;


public class FinancialListDto : BaseDto<FinancialListDto, Financial, long>
{
    public decimal TotalPayablePrice { get; set; }
    public decimal GrantPrice { get; set; }
    public decimal AnalyzePrice { get; set; }
    public decimal AdditionalPrice { get; set; }
    public decimal Tax { get; set; }
}


public class FinancialBriefListDto : BaseDto<FinancialBriefListDto, Financial, long>
{
    public long PaymentId { get; set; }
    public string Title { get; set; }
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
}