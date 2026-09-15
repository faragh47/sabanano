using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeleteCustomer;

public record DeleteCustomerCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteCustomerCommand,long>
{
    private readonly IRepository<Customer> _CustomerRepository;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context,
        IRepository<Customer> CustomerRepository)
    {
        _CustomerRepository = CustomerRepository;
    }

    public async Task<long> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _CustomerRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

        await _CustomerRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
