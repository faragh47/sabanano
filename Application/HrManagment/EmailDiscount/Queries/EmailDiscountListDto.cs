using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Email;
using CleanArchitecture.Domain.Entities.HrManagment;
using System.ComponentModel.DataAnnotations;

public class EmailDiscountListDto : BaseDto<EmailDiscountListDto, EmailDiscount, int>
{
    [MaxLength(100)]
    public string Email { get; set; }
}


public class EmailDiscountBriefListDto : BaseDto<EmailDiscountBriefListDto, EmailDiscount, int>
{
    [MaxLength(100)]
    public string Email { get; set; }
}
