//using CleanArchitecture.Application.Common.Exceptions;
//using CleanArchitecture.Application.Common.Interfaces;
//using CleanArchitecture.Domain.Entities;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;
//using CleanArchitecture.Domain.Entities.HrManagment;

//using Common.Exceptions;
//using Data.Contracts;
//using MediatR;
//using Microsoft.EntityFrameworkCore;


//public record DeleteFinancialDetailCommand : IRequest<long>
//{
//    public long Id { get; set; }
//}

//public class DeleteFinancialDetailCommandHandler : IRequestHandler<DeleteFinancialDetailCommand, long>
//{
//    private readonly IRepository<FinancialDetail> _repository;
//    private readonly ITimingValidatorService _timingValidatorService;
//    public DeleteFinancialDetailCommandHandler(IApplicationDbContext context,
//        IRepository<FinancialDetail> repository)
//    {
//        _repository = _repository;
//    }

//    public async Task<long> Handle(DeleteFinancialDetailCommand request, CancellationToken cancellationToken)
//    {
//        var entity = await _repository.TableNoTracking
//                    .FirstOrDefaultAsync(x => x.Id == request.Id);

//        if (entity == null)
//        {
//            throw new NotFoundException(nameof(FinancialDetail), request.Id);
//        }

//        await _repository.DeleteAsync(entity, cancellationToken);
//        //entity.AddDomainFinancialDetail(new TodoItemDeletedFinancialDetail(entity));
//        //await _context.SaveChangesAsync(cancellationToken);

//        return request.Id;
//    }
//}
