using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.Orders.Queries;

public class OrderHistoryDto : BaseDto<OrderHistoryDto, OrderHistory, long>
{
    public string TechnicalComment { get; set; }
    public long OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}   