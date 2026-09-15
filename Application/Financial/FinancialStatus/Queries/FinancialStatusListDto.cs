using AutoMapper;
using AutoMapper.Configuration;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class FinancialStatusListDto : BaseDto<FinancialStatusListDto, FinancialStatus, int>
{
    public string Title { get; set; }

    //public override void CustomMappings(IMappingExpression<FinancialStatus, FinancialStatusListDto> mapping)
    //{
    //    mapping.ForMember(x=>x., opt => opt.Ignore());
    //}
}


public class FinancialStatusBriefListDto : BaseDto<FinancialStatusBriefListDto, FinancialStatus, int>
{
    public string Title { get; set; }
}
