using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class AccountListDto : BaseDto<AccountListDto, Account,long>
{
    public string Title { get; set; }
    public decimal Credit { get; set; }
    public string ShebaCode { get; set; }
    public decimal Balance { get; set; }
    public decimal Debit { get; set; }
}


public class AccountBriefListDto : BaseDto<AccountBriefListDto, Account, long>
{
    public string Title { get; set; }
    public string ShebaCode { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public decimal Debit { get; set; }
}
