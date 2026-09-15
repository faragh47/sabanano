using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeletePerson;

public record DeletePersonCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeletePersonCommand,long>
{
    private readonly IRepository<Person> _personRepository;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context,
        IRepository<Person> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<long> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _personRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Person), request.Id);
        }

        await _personRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
