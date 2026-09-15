//using System;
//using AutoMapper;
//using CleanArchitecture.Application.Financial.Financial.Accountant;
//using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;

//using Data.Contracts;

//    public class OrderAccountantService : IOrderAccountantService
//    {
//        private readonly IRepository<Dinner> _dinnerRepository;
//        private readonly IRepository<Order> _orderRepository;
//    private readonly IRepository<Account> _accountRepository;
//        private readonly IRepository<Financial> _financialRepository;
//        private readonly IRepository<FinancialDetail> _financialDetailRepository;
//    private readonly IMapper _mapper;

//    public OrderAccountantService(IRepository<Dinner> DinnerRepository,
//                                  IRepository<Account> accountRepository,
//                                  IRepository<Financial> financialRepository,
//                                  IRepository<FinancialDetail> financialDetailRepository,
//                                  IMapper mapper,
//                                  IRepository<Order> orderRepository)
//    {
//        _dinnerRepository = DinnerRepository;
//        _accountRepository = accountRepository;
//        _financialRepository = financialRepository;
//        _financialDetailRepository = financialDetailRepository;
//        _mapper = mapper;
//        _orderRepository = orderRepository;
//    }
//    public async Task<FinancialListDto> CaluclateFinancial(Order order,CancellationToken cancellationToken)
//        {
//            var calcuator =new CalculatorStorage(order);

//            if (order.ServiceTypeId == ServiceType.Reservation.Id)
//            {
//                var result =await calcuator.Calculate(new ReservasionCalculator(), cancellationToken);
//                return result;
//            }
//            else
//            {
//                var result = await calcuator.Calculate(new DinnerCalculator(_dinnerRepository,
//                                                        _financialRepository,
//                                                        _accountRepository,
//                                                        _mapper,
//                                                        _financialDetailRepository,
//                                                        _orderRepository), cancellationToken);
//                return result;
//            }
//        }
//    }

