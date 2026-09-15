//using CleanArchitecture.Application.Common.Exceptions;
//using CleanArchitecture.Application.Common.Interfaces;
//using CleanArchitecture.Domain.Entities;
//using CleanArchitecture.Domain.Entities.FinancialAggregate;
//using CleanArchitecture.Domain.Entities.HrManagment;

//using Common.Exceptions;
//using Data.Contracts;
//using MediatR;
//using Microsoft.EntityFrameworkCore;


//public record DeleteFinancialCommand : IRequest<long>
//{
//    public long Id { get; set; }
//}

//public class DeleteFinancialCommandHandler : IRequestHandler<DeleteFinancialCommand, long>
//{
//    private readonly IRepository<Financial> _repository;
//    private readonly ITimingValidatorService _timingValidatorService;
//    public DeleteFinancialCommandHandler(IApplicationDbContext context,
//        IRepository<Financial> repository)
//    {
//        _repository = _repository;
//    }

//    public async Task<long> Handle(DeleteFinancialCommand request, CancellationToken cancellationToken)
//    {
//        var entity = await _repository.TableNoTracking
//                    .FirstOrDefaultAsync(x => x.Id == request.Id);

//        if (entity == null)
//        {
//            throw new NotFoundException(nameof(Financial), request.Id);
//        }

//        await _repository.DeleteAsync(entity, cancellationToken);
//        //entity.AddDomainFinancial(new TodoItemDeletedFinancial(entity));
//        //await _context.SaveChangesAsync(cancellationToken);

//        return request.Id;
//    }
//}
