//using System;
//using AutoMapper;
//using CleanArchitecture.Application.Common.Mappings;
//using CleanArchitecture.Application.Financial.Financial.Accountant;
//using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;


//using Common.Exceptions;
//using Data.Contracts;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//public record class CreateAndGetFinancialCommand : BaseRecordDto<CreateAndGetFinancialCommand, Financial, long>, IRequest<FinancialListDto>
//{
//    public long OrderId { get; set; }
//}

//public class CreateAndGetFinancailCommandHandler : IRequestHandler<CreateAndGetFinancialCommand, FinancialListDto>
//{
//    private readonly IRepository<Financial> _repository;
//    private readonly IMapper _mapper;
//    private readonly IOrderAccountantService _orderAccountantService;

//    public CreateAndGetFinancailCommandHandler(IRepository<Financial> repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;
//    }

//    public async Task<FinancialListDto> Handle(CreateAndGetFinancialCommand request, CancellationToken cancellationToken)
//    {
//        //var order=await _orderRepository.TableNoTracking
//        //                .Include(x=>x.Customer)
//        //                .FirstOrDefaultAsync(x=>x.Id==request.OrderId);

//        //if(order is null)
//        //    throw new BadRequestException("سفارش یافت نشد");

//        var financial=await _orderAccountantService.CaluclateFinancial(order,cancellationToken);

//        // entity.AddDomainFinancial(new ProviderCreatedFinancial(entity));

//        //_context.TodoItems.Add(entity);

//        return financial;
//    }
//}

