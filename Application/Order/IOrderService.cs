using Common;

namespace CleanArchitecture.Application.Orders;

public interface IOrderService
{
    public int InitialCount();
    public bool InitialExist();
}