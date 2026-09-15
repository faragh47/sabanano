using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzeDeviceAttributeDetailCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzeDeviceAttributeDetailCommandHandler : IRequestHandler<DeleteAnalyzeDeviceAttributeDetailCommand, int>
{
    private readonly IRepository<AnalyzeDeviceAttributeDetail> _repository;
    public DeleteAnalyzeDeviceAttributeDetailCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzeDeviceAttributeDetail> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzeDeviceAttributeDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzeDeviceAttributeDetail), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzeDeviceAttributeDetail(new TodoItemDeletedAnalyzeDeviceAttributeDetail(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
