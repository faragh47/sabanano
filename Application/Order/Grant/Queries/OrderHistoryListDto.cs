using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order;
using CleanArchitecture.Domain.Entities.Orders;

namespace CleanArchitecture.Application.TodoItems.Queries.OrderHistorys;

public class OrderHistoryListDto : BaseDto<OrderHistoryListDto, OrderHistory,long>
{
    public string NationalCode { get; set; }
}


