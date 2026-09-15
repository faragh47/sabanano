using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record DeleteDiscountCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, long>
{
    private readonly IRepository<Discount> _repository;

    public DeleteDiscountCommandHandler(IApplicationDbContext context,
        IRepository<Discount> repository)
    {
        _repository = _repository;
    }

    public async Task<long> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Discount), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
