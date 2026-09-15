//using AutoMapper;
//using CleanArchitecture.Application.Common.Exceptions;
//using CleanArchitecture.Application.Common.Interfaces;
//using CleanArchitecture.Application.Common.Mappings;
//using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
//using CleanArchitecture.Domain.Entities;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;
//using CleanArchitecture.Domain.Entities.HrManagment;
//using Data.Contracts;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace CleanArchitecture.Application.People.Commands.UpdateAccount;

//public record UpdateAccountCommand : BaseRecordDto<UpdateAccountCommand, Account, long>, IRequest<long>
//{
//    public string Title { get; set; }
//    public decimal Credit { get; set; }
//    public string ShebaCode { get; set; }
//    public decimal Balance { get; set; }
//    public decimal Debit { get; set; }
//    public override void CustomMappings(IMappingExpression<UpdateAccountCommand, Account> mapping)
//    {
//        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
//    }
//}

//public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateAccountCommand, long>
//{
//    private readonly IRepository<Account> _repository;
//    private readonly IMapper _mapper;
//    public UpdateTodoItemCommandHandler(IRepository<Account> repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;
//        _timingValidatorService = timingValidatorService;
//    }

//    public async Task<long> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
//    {
//        var entity = await _repository.TableNoTracking
//                            .FirstOrDefaultAsync(x => x.Id == request.Id);

//        //await _timingValidatorService.CheckTimingOfOrder(entity.Order);

//        //var Account = request.ToEntity(_mapper, entity);

//        await _repository.UpdateAsync(entity, cancellationToken);

//        return entity.Id;
//    }


//}
