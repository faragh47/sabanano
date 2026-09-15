using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Utilities;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class PersonDiscountListDto : BaseDto<PersonDiscountListDto, PersonDiscount,long>
{
    public long PersonId { get; set; }
    public DiscountBriefListDto DiscountId { get; set; }
    //public ServceTypeBriefListDto Deliveryman{ get; set; }
}


public class PersonDiscountBriefListDto : BaseDto<PersonDiscountBriefListDto, PersonDiscount, long>
{
    public List<DiscountAmount> Amounts { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public string ExpirationDate { get; set; }
    public int Order { get; set; }
    public int Count { get; set; }
    public decimal? Amount { get; set; }
    public int? Percent { get; set; }
    public decimal? Max { get; set; }
    public List<DiscountIncrease> Increases { get; set; }
    public PersonDiscountBriefListDto()
    {
        Amounts = new();
    }

    public override void CustomMappings(IMappingExpression<PersonDiscount, PersonDiscountBriefListDto> mappingExpression)
    {
        mappingExpression.ForMember(dest => dest.ExpirationDate, conf => conf.MapFrom(src => src.Discount.ExpirationDate.ToPersianDate()));
        mappingExpression.ForMember(dest => dest.Order, conf => conf.MapFrom(src => src.Discount.Useds.Count(x=>x.PersonId==src.PersonId)));
        mappingExpression.ForMember(dest => dest.Code, conf => conf.MapFrom(src => src.Discount.Code));
        mappingExpression.ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Discount.Description));
        mappingExpression.ForMember(dest => dest.Count, conf => conf.MapFrom(src => src.Discount.Count));
        mappingExpression.ForMember(dest => dest.Increases, conf => conf.MapFrom(src => src.Discount.Increases));
        mappingExpression.ForMember(dest => dest.Max, conf => conf.MapFrom(src => src.Discount.Max));
        mappingExpression.ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.Discount.Amount));
        mappingExpression.ForMember(dest => dest.Percent, conf => conf.MapFrom(src => src.Discount.Percent));
    }
}

public class DiscountAmount
{
    public int? Percent { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Max { get; set; }
}