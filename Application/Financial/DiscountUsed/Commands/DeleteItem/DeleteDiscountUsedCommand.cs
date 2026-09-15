using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeleteDiscountUsed;

public record DeleteDiscountUsedCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteDiscountUsedCommandHandler : IRequestHandler<DeleteDiscountUsedCommand, long>
{
    private readonly IRepository<DiscountUsed> _repository;

    public DeleteDiscountUsedCommandHandler(IApplicationDbContext context,
        IRepository<DiscountUsed> repository)
    {
        _repository = _repository;
    }

    public async Task<long> Handle(DeleteDiscountUsedCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(DiscountUsed), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
