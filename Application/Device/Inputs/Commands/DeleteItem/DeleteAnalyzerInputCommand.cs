using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzeDeviceInputCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzeDeviceInputCommandHandler : IRequestHandler<DeleteAnalyzeDeviceInputCommand, int>
{
    private readonly IRepository<AnalyzeDeviceInput> _repository;
    public DeleteAnalyzeDeviceInputCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzeDeviceInput> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzeDeviceInputCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzeDeviceInput), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzeDeviceInput(new TodoItemDeletedAnalyzeDeviceInput(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
