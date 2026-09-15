using AutoMapper;
using AutoMapper.Configuration;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class PaymentTypeListDto : BaseDto<PaymentTypeListDto, PaymentType, int>
{
    public string Title { get; set; }

    //public override void CustomMappings(IMappingExpression<PaymentType, PaymentTypeListDto> mapping)
    //{
    //    mapping.ForMember(x=>x., opt => opt.Ignore());
    //}
}


public class PaymentTypeBriefListDto : BaseDto<PaymentTypeBriefListDto, PaymentType, int>
{
    public string Title { get; set; }
}
