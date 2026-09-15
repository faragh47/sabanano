using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeleteAddress;

public record DeleteAddressCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, long>
{
    private readonly IRepository<Address> _AddressRepository;

    public DeleteAddressCommandHandler(IApplicationDbContext context,
        IRepository<Address> AddressRepository)
    {
        _AddressRepository = AddressRepository;
    }

    public async Task<long> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _AddressRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Address), request.Id);
        }

        await _AddressRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
