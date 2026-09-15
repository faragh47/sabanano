using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzerDeviceCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzerDeviceCommandHandler : IRequestHandler<DeleteAnalyzerDeviceCommand, int>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    public DeleteAnalyzerDeviceCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzerDevice> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzerDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzerDevice), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzerDevice(new TodoItemDeletedAnalyzerDevice(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
