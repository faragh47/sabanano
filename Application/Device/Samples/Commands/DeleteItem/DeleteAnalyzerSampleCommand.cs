using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzerDeviceSampleCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzerDeviceSampleCommandHandler : IRequestHandler<DeleteAnalyzerDeviceSampleCommand, int>
{
    private readonly IRepository<AnalyzerDeviceSample> _repository;
    public DeleteAnalyzerDeviceSampleCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzerDeviceSample> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzerDeviceSampleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzerDeviceSample), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzerDeviceSample(new TodoItemDeletedAnalyzerDeviceSample(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
