using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Email;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteEmailDiscountCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteEmailDiscountCommandHandler : IRequestHandler<DeleteEmailDiscountCommand, int>
{
    private readonly IRepository<EmailDiscount> _repository;
    public DeleteEmailDiscountCommandHandler(IApplicationDbContext context,
        IRepository<EmailDiscount> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteEmailDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(EmailDiscount), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEmailDiscount(new TodoItemDeletedEmailDiscount(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
