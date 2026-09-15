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


//public record UpdateFinancialDetailCommand : BaseRecordDto<UpdateFinancialDetailCommand, FinancialDetail, long>, IRequest<long>
//{
//    public long FinancialId { get; set; }
//    public decimal Price { get; set; }
//    public string Title { get; set; }
//    public string Description { get; set; }
//    public override void CustomMappings(IMappingExpression<UpdateFinancialDetailCommand, FinancialDetail> mapping)
//    {
//        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
//    }
//}

//public class UpdateFinancialDetailCommandHandler : IRequestHandler<UpdateFinancialDetailCommand, long>
//{
//    private readonly IRepository<FinancialDetail> _repository;
//    private readonly IMapper _mapper;
//    private readonly ITimingValidatorService _timingValidatorService;
//    public UpdateFinancialDetailCommandHandler(IRepository<FinancialDetail> repository, IMapper mapper,
//                                        ITimingValidatorService timingValidatorService)
//    {
//        _repository = repository;
//        _mapper = mapper;
//        _timingValidatorService = timingValidatorService;
//    }

//    public async Task<long> Handle(UpdateFinancialDetailCommand request, CancellationToken cancellationToken)
//    {
//        var entity = await _repository.TableNoTracking
//                            .FirstOrDefaultAsync(x => x.Id == request.Id);

//        //await _timingValidatorService.CheckTimingOfOrder(entity.Order);

//        //var FinancialDetail = request.ToEntity(_mapper, entity);

//        await _repository.UpdateAsync(entity, cancellationToken);

//        return entity.Id;
//    }


//}
