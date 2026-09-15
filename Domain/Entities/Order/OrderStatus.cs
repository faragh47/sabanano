using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Domain.ValueObjects;

public class OrderStatus : ValueObject<int>
{
    public string Title { get; set; }

    private OrderStatus(string title, int id)
    {
        Title = title;
        Id = id;
    }

    public OrderStatus()
    {
    }

    public static OrderStatus Initial => new("سفارش اولیه", 1);
    public static OrderStatus SendToTechnicalExpert => new("ارسال جهت تایید کارشناس", 2);
    public static OrderStatus TechnicalConfirm => new("تایید کارشناسی", 3);
    public static OrderStatus SendToFinancial => new("در انتظار پرداخت", 4);
    public static OrderStatus WaitingForFinancial => new("درانتظار تایید مالی", 5);
    public static OrderStatus FinancialConfirmed => new("تایید مالی", 7);
    public static OrderStatus Proccessing => new("در حال انجام", 8);
    public static OrderStatus Completed => new("دریافت نتایج", 9);
    public static OrderStatus Canceled => new("کنسل شده", 10);

    public static IEnumerable<OrderStatus> Items
    {
        get
        {
            yield return Initial;
            yield return SendToTechnicalExpert;
            yield return TechnicalConfirm;
            yield return Proccessing;
            yield return Completed;
        }
    }

    public static OrderStatus GetItem(string item, IEnumerable<OrderStatus> Items)
    {
        var result = Items.Where(x => x.Title == item).FirstOrDefault();
        return result;
    }

    public static long FindId(string title)
    {
        var item = GetItem(title, Items);
        return Convert.ToInt64(item);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
    }
}