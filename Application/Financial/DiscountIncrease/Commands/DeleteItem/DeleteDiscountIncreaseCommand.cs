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

namespace CleanArchitecture.Application.People.Commands.DeleteDiscountIncrease;

public record DeleteDiscountIncreaseCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteDiscountIncreaseCommandHandler : IRequestHandler<DeleteDiscountIncreaseCommand, long>
{
    private readonly IRepository<DiscountIncrease> _repository;

    public DeleteDiscountIncreaseCommandHandler(IApplicationDbContext context,
        IRepository<DiscountIncrease> repository)
    {
        _repository = _repository;
    }

    public async Task<long> Handle(DeleteDiscountIncreaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(DiscountIncrease), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
