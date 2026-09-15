using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzeDeviceServiceCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzeDeviceServiceCommandHandler : IRequestHandler<DeleteAnalyzeDeviceServiceCommand, int>
{
    private readonly IRepository<AnalyzeDeviceService> _repository;
    public DeleteAnalyzeDeviceServiceCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzeDeviceService> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzeDeviceServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzeDeviceService), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzeDeviceService(new TodoItemDeletedAnalyzeDeviceService(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
