//using System;
//using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;



//namespace CleanArchitecture.Application.Financial.Financial.Accountant
//{
//    public class CalculatorStorage
//    {
//        public CalculatorStorage(Order order)
//        {
//            currentOrder = order;
//        }
//        private Order currentOrder;

//        public Order CurrentOrder { get => currentOrder; set => currentOrder = value; }

//        public Task<FinancialListDto?> Calculate(IFinancialCalculator financialCalculator,CancellationToken cancellationToken)
//        {
//            return financialCalculator.Calculate(currentOrder, cancellationToken);
//        }

//    }
//}


