using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Queries;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class OrderDto : BaseDto<OrderDto, Order, long>
{
    public string FullOrder { get; set; }
    public string PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public class OrderBriefDto : BaseDto<OrderBriefDto, Order, long>
{
    public string? Created { get; set; }
    public string? CreatedBy { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string TrackingCode { get; set; }
    public decimal TotalPrice { get; set; }
    public string? ProcessingTime { get; set; }
    public int FinancialStatusId { get; set; }
    public string PaymentImageFileName { get; set; }
    public string ResultImageFileName { get; set; }
    public OrderHistoryDto LastHistory { get; set; }
    public FinancialListDto Financial { get; set; }
    public List<OrderAnalyzeBriefDto> Analyzes { get; set; } = new();
    public AnalyzerDeviceBriefListDto AnalyzerDevice { get; set; }

    public override void CustomMappings(IMappingExpression<Order, OrderBriefDto> mapping)
    {
        mapping.ForMember(dest => dest.AnalyzerDevice,
                conf =>
                    conf.MapFrom(src => src.OrderAnalyzes.FirstOrDefault().AnalyzerDevice))
            .ForMember(dest => dest.Created,
                conf =>
                    conf.MapFrom(src => src.Created.ToPersianLongDate()))
            .ForMember(dest => dest.ProcessingTime,
                conf =>
                    conf.MapFrom(src => GetProcessingDate(src.ProcessingTime, src.Created)))
            .ForMember(dest => dest.TotalPrice,
                conf =>
                    conf.MapFrom(src => src.Financial != null ? src.Financial.TotalPayablePrice : 0))
            .ForMember(dest => dest.FinancialStatusId,
                conf =>
                    conf.MapFrom(src => src.Financial != null ? src.Financial.StatusId : 0))
            .ForMember(dest => dest.PaymentImageFileName,
                conf =>
                    conf.MapFrom(src => src.Financial != null ? src.Financial.Payment.Image.Title : ""))
            .ForMember(dest => dest.ResultImageFileName,
                conf =>
                    conf.MapFrom(src => src.Image != null ? src.Image.Title : ""))
            .ForMember(dest => dest.Analyzes,
                conf =>
                    conf.MapFrom(src => src.OrderAnalyzes));
    }

    public static string GetProcessingDate(double? processingDate, DateTime created)
    {
        if (processingDate is not null)
        {
            string date = created.AddDays(Convert.ToInt64(processingDate)).ToPersianLongDate();
            return date;
        }
        else
            return "";
    }

    public record OrderExportDto
    {
        public long RowIndex { get; set; }              // ردیف
        public string? SampleCode { get; set; }         // کد نمونه
        public string? CustomerName { get; set; }       // نام مشتری/ سازمان
        public string? Date { get; set; }               // تاریخ
        public string? Analysis { get; set; }           // آنالیز
        public string? TypeNormalUrgent { get; set; }   // نوع (عادی- فوری)
        public int? Count { get; set; }                 // تعداد
        public decimal? AnalysisCost { get; set; }      // هزینه آنالیز (تومان)
        public decimal? SetadPaid { get; set; }         // هزینه پرداختی ستاد (تومان)
        public decimal? TotalCost { get; set; }         // هزینه کل (تومان)
        public decimal? Tax { get; set; }               // مالیات (تومان)
        public decimal? NetAfterGrantTax { get; set; }  // هزینه کل پس از کسر گرنت و افزودن مالیات (تومان)
        public decimal? TotalPaid { get; set; }         // جمع کل پرداختی
        public string? ResultSent { get; set; }         // نتیجه (ارسال شده- ارسال نشده)
        public string? StatusPaid { get; set; }         // وضعیت (پرداخت شده- پرداخت نشده)
        public string? PartnerLab { get; set; }         // آزمایشگاه همکار
        public decimal? PartnerPaid { get; set; }       // پرداختی همکار
        public string? PaidOrNot { get; set; }          // پرداخت شده- نشده
        public string? NationalId { get; set; }         // کد ملی
        public string? Phone { get; set; }              // شماره تلفن
        public string? Email { get; set; }              // ایمیل
        public string? Description { get; set; }        // توضیحات
        public string? DocCode { get; set; }            // کد مدرک: F-46/00
    }

}