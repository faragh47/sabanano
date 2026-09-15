using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeletePolygon;

public record DeletePolygonCommand : IRequest<long>
{
    public long Id { get; set; }
}

public class DeletePolygonCommandHandler : IRequestHandler<DeletePolygonCommand,long>
{
    private readonly IRepository<Polygon> _PolygonRepository;

    public DeletePolygonCommandHandler(IApplicationDbContext context,
        IRepository<Polygon> PolygonRepository)
    {
        _PolygonRepository = PolygonRepository;
    }

    public async Task<long> Handle(DeletePolygonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _PolygonRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Polygon), request.Id);
        }

        await _PolygonRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new PolygonDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
