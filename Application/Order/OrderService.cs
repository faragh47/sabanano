using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Orders;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _ordersRepository;
    private readonly ICurrentUserService _currentUserService;

    public OrderService(IRepository<Order> ordersRepository, ICurrentUserService currentUserService)
    {
        _ordersRepository = ordersRepository;
        _currentUserService = currentUserService;
    }

    public int InitialCount()
    {
        return _ordersRepository.TableNoTracking.Count(x => x.CreatedBy == _currentUserService.UserId
                                                            && x.OrderStatus.Id == OrderStatus.Initial.Id);
    }

    public bool InitialExist()
    {
        return _ordersRepository.TableNoTracking.Any(x => x.CreatedBy == _currentUserService.UserId
                                                          && x.OrderStatus.Id == OrderStatus.Initial.Id);
    }
}