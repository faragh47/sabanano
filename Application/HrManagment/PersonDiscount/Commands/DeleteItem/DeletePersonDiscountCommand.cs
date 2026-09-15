using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeletePersonDiscount;

public record DeletePersonDiscountCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeletePersonDiscountCommandHandler : IRequestHandler<DeletePersonDiscountCommand, long>
{
    private readonly IRepository<PersonDiscount> _repository;

    public DeletePersonDiscountCommandHandler(IApplicationDbContext context,
        IRepository<PersonDiscount> repository)
    {
        _repository = _repository;
    }

    public async Task<long> Handle(DeletePersonDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(PersonDiscount), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
