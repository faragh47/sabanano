using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;

using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteAnalyzeDeviceResponseCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteAnalyzeDeviceResponseCommandHandler : IRequestHandler<DeleteAnalyzeDeviceResponseCommand, int>
{
    private readonly IRepository<AnalyzeDeviceResponse> _repository;
    public DeleteAnalyzeDeviceResponseCommandHandler(IApplicationDbContext context,
        IRepository<AnalyzeDeviceResponse> repository)
    {
        _repository = _repository;
    }

    public async Task<int> Handle(DeleteAnalyzeDeviceResponseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AnalyzeDeviceResponse), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainAnalyzeDeviceResponse(new TodoItemDeletedAnalyzeDeviceResponse(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
