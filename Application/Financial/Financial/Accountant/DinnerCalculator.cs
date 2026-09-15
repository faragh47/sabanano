//using System;
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using CleanArchitecture.Application.Financial.Financial.Accountant;
//using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;

//using CleanArchitecture.Domain.ValueObjects;
//using Common.Exceptions;
//using Data.Contracts;
//using Microsoft.EntityFrameworkCore;

//public class DinnerCalculator : IFinancialCalculator
//{
//    private readonly IRepository<Account> _accountRepository;
//    private readonly IRepository<Financial> _financialRepository;
//    private readonly IRepository<FinancialDetail> _financialDetailRepository;
//    private readonly IMapper _mapper;

//    public DinnerCalculator(
//                            IRepository<Financial> financialRepository,
//                            IRepository<Account> accountRepository,
//                            IMapper mapper,
//                            IRepository<FinancialDetail> financialDetailRepository)
//    {
//        _accountRepository = accountRepository;
//        _financialRepository = financialRepository;
//        _mapper = mapper;
//        _financialDetailRepository = financialDetailRepository;
//    }

//    public async Task<FinancialListDto?> Calculate(Order order, CancellationToken cancellationToken)
//    {
//        var orderEntity =await _orderRepository.TableNoTracking
//          .Include(x=>x.Customer).FirstOrDefaultAsync(x=>x.Id==order.Id);

//        if (orderEntity is null)
//            throw new BadRequestException("سفارش یافت نشد");

//        order = orderEntity;

//        var dinners = await _dinnerRepository.TableNoTracking
//                          .Include(x => x.RestaurantFood)
//                          .Include(x => x.Restaurant)
//                          .ToListAsync();

//        var account = await _accountRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == order.Customer.AccountId);

//        var restaurant = dinners.Select(x => x.Restaurant).FirstOrDefault();

//        decimal sumPrice = 0;

//        List<FinancialDetail> details = new List<FinancialDetail>();

//        foreach (var item in dinners)
//        {
//            details.Add(new FinancialDetail()
//            {
//                Title = item.RestaurantFood.Title,
//                Price = item.RestaurantFood.Price,
//                Description= item.RestaurantFood.Title
//            }); ;
//            sumPrice += item.RestaurantFood.Price;
//        }

//        details.Add(new FinancialDetail()
//        {
//            Title = "هزینه ارسال",
//            Description = "هزینه ارسال",
//            Price = restaurant.DeliveryPrice
//        });

//        sumPrice += restaurant.DeliveryPrice;

//        if (restaurant.Tax is not null)
//        {
//            sumPrice += Convert.ToDecimal(restaurant.Tax);

//            details.Add(new FinancialDetail()
//            {
//                Title = "هزینه مالیات",
//                Description = "هزینه مالیات",
//                Price = Convert.ToDecimal(restaurant.Tax)
//        });
//        }
//        if (restaurant.PackagingPrice is not null)
//        {
//            sumPrice += Convert.ToDecimal(restaurant.PackagingPrice);

//            details.Add(new FinancialDetail()
//            {
//                Title = "هزینه بسته بندی",
//                Description = "هزینه بسته بندی",
//                Price = Convert.ToDecimal(restaurant.PackagingPrice)
//            });
//        }

//        var financial = await _financialRepository.TableNoTracking
//                        .FirstOrDefaultAsync(x => x.Id == order.FinancialId);

//        if (financial is null)
//        {
//            financial = new Financial()
//            {
//                StatusId = FinancialStatus.WaitForPayment.Id,
//                Title = "شماره سفارش: " + order.OrderNumber,
//                TotalPrice = sumPrice
//            };

//            await _financialRepository.AddAsync(financial, cancellationToken);

//            foreach (var financialDetail in details)
//            {
//                financialDetail.FinancialId = financial.Id;
//                await _financialDetailRepository.AddAsync(financialDetail, cancellationToken);
//            }
//        }
//        else if(order.OrderStatusId==OrderStatus.AwaitingforFinancial.Id)
//        {
//          var RemoveDetails=  _financialDetailRepository.TableNoTracking.Where(x=>x.FinancialId==financial.Id);

//            foreach (var item in RemoveDetails)
//            {
//                await _financialDetailRepository.DeleteAsync(item,cancellationToken);
//            }

//            foreach (var financialDetail in details)
//            {
//                financialDetail.FinancialId = financial.Id;
//                await _financialDetailRepository.AddAsync(financialDetail, cancellationToken);
//            }

//            financial.TotalPrice = sumPrice;
//            await _financialRepository.UpdateAsync(financial, cancellationToken);
//        }

//         var result = await _financialRepository.TableNoTracking
//                        .ProjectTo<FinancialListDto>(_mapper.ConfigurationProvider)
//                        .FirstOrDefaultAsync(x => x.Id == financial.Id);

//        return result;
//    }
//}

