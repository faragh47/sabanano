using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeleteAccount;

public record DeleteAccountCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, long>
{
    private readonly IRepository<Account> _repository;
    public DeleteAccountCommandHandler(IApplicationDbContext context,
        IRepository<Account> repository)
    {
        _repository = _repository;
    }

    public async Task<long> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Account), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAccount(new TodoItemDeletedAccount(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
