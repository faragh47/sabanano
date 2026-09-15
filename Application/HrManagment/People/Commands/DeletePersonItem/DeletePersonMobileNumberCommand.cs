using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeletePerson;

public record DeletePersonMobileNumberCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeletePersonMobileNumberCommandHandler : IRequestHandler<DeletePersonMobileNumberCommand, long>
{ 
    private readonly IRepository<PersonMobileNumber> _personMobileRepository;

    public DeletePersonMobileNumberCommandHandler(IApplicationDbContext context,
        IRepository<PersonMobileNumber> personMobileRepository)
    {
        _personMobileRepository = personMobileRepository;
    }

    public async Task<long> Handle(DeletePersonMobileNumberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _personMobileRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Person), request.Id);
        }

        await _personMobileRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
