using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;

public class DiscountListDto : BaseDto<DiscountListDto, Discount, long>
{
    public string ExpirationDate { get; set; }
    public int? Count { get; set; }
    public int DistcountTypeId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public string Description { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Max { get; set; }
    public int? Percent { get; set; }

    public override void CustomMappings(IMappingExpression<Discount, DiscountListDto> mapping)
    {
        mapping.ForMember(dest => dest.ExpirationDate,
            conf => conf.MapFrom(src => src.ExpirationDate.ToPersianDate()));
    }
}


public class DiscountBriefListDto : BaseDto<DiscountBriefListDto, Discount, long>
{
    public string ExpirationDate { get; set; }
    public int? Count { get; set; }
    public int DistcountTypeId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public string Description { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Max { get; set; }
    public int? Percent { get; set; }
    public override void CustomMappings(IMappingExpression<Discount, DiscountBriefListDto> mapping)
    {
        mapping.ForMember(dest => dest.ExpirationDate,
         conf => conf.MapFrom(src => src.ExpirationDate.ToPersianDate()));
    }
}
