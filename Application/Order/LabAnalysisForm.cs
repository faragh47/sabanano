using System;
using System.Collections.Generic;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities.Grants;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using CleanArchitecture.Domain.Entities.Orders;
using Microsoft.VisualBasic;
using Financial = CleanArchitecture.Domain.Entities.FinancialAggregate.Financial;

public class LabAnalysisForm
{
    public string DocumentNumber { get; set; }
    public string TrackingCode { get; set; }
    public DateTime AcceptanceDate { get; set; }
    public string FormTitle { get; set; }
    public CustomerInfo Customer { get; set; }
    public GrantInfo Grant { get; set; }
    public CompanyInfo Company { get; set; }
    public List<DocumentOrderAnalyze> Analyzes { get; set; }
    public string AdditionalComments { get; set; }
    public CostsAndPayments PaymentDetails { get; set; }
    public LabCompletionInfo LabCompletionDetails { get; set; }
    public string FooterNotes { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
}

public class GrantInfo : BaseDto<GrantInfo, Grant, long>
{
    public bool HasGrant { get; set; }
    public string NationalCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? UniversityName { get; set; }
    public string? TelNumber { get; set; }
}

public class CompanyInfo : BaseDto<CompanyInfo, Company, long>
{
    public string NationalCode { get; set; }
    public string RegisterCode { get; set; }
    public string MobileNumber { get; set; }
    public string EconomicNumber { get; set; }
}

public class DocumentOrderAnalyze : BaseDto<GrantInfo, Grant, long>
{
    public int RowNumber { get; set; }
    public string SampleName { get; set; }
    public string SampleCode { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
}

// Costs and Payments
public class CostsAndPayments : BaseDto<CostsAndPayments, Financial, long>
{
    public decimal TotalPayablePrice { get; set; }
    public decimal AnalyzePrice { get; set; }
    public decimal Tax { get; set; }
    public decimal GrantPrice { get; set; }
}

public class LabCompletionInfo
{
    public bool CanPerformTest { get; set; }
    public string ExpertOpinion { get; set; }
}