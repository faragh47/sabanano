using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeleteOrder;

public record DeleteOrderCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, long>
{
    private readonly IRepository<Order> _OrderRepository;

    public DeleteOrderCommandHandler(IApplicationDbContext context,
        IRepository<Order> OrderRepository)
    {
        _OrderRepository = OrderRepository;
    }

    public async Task<long> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _OrderRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        await _OrderRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
