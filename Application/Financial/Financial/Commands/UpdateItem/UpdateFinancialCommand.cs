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


//public record UpdateFinancialCommand : BaseRecordDto<UpdateFinancialCommand, Financial, long>, IRequest<long>
//{
//    public long PaymentId { get; set; }
//    public string Title { get; set; }
//    public decimal TotalPrice { get; set; }
//    public int StatusId { get; set; }
//    public override void CustomMappings(IMappingExpression<UpdateFinancialCommand, Financial> mapping)
//    {
//        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
//    }
//}

//public class UpdateFinancialCommandHandler : IRequestHandler<UpdateFinancialCommand, long>
//{
//    private readonly IRepository<Financial> _repository;
//    private readonly IMapper _mapper;
//    private readonly ITimingValidatorService _timingValidatorService;
//    public UpdateFinancialCommandHandler(IRepository<Financial> repository, IMapper mapper,
//                                        ITimingValidatorService timingValidatorService)
//    {
//        _repository = repository;
//        _mapper = mapper;
//        _timingValidatorService = timingValidatorService;
//    }

//    public async Task<long> Handle(UpdateFinancialCommand request, CancellationToken cancellationToken)
//    {
//        var entity = await _repository.TableNoTracking.
//                            FirstOrDefaultAsync(x => x.Id == request.Id);

//        //await _timingValidatorService.CheckTimingOfOrder(entity.Order);

//        //var Financial = request.ToEntity(_mapper, entity);

//        await _repository.UpdateAsync(entity, cancellationToken);

//        return entity.Id;
//    }


//}
