using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzeDeviceAttributeCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzeDeviceAttributeCommandHandler : IRequestHandler<DeleteAnalyzeDeviceAttributeCommand, int>
{
    private readonly IRepository<AnalyzeDeviceAttribute> _repository;
    public DeleteAnalyzeDeviceAttributeCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzeDeviceAttribute> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzeDeviceAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzeDeviceAttribute), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzeDeviceAttribute(new TodoItemDeletedAnalyzeDeviceAttribute(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
