using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;


using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record UpdateAnalyzerDevicePriceCommand : BaseRecordDto<UpdateAnalyzerDevicePriceCommand, AnalyzerDevice, int>, IRequest<int>
{
    public decimal Price { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateAnalyzerDevicePriceCommand, AnalyzerDevice> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateAnalyzerDevicePriceCommandHandler : IRequestHandler<UpdateAnalyzerDevicePriceCommand, int>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IMapper _mapper;
    public UpdateAnalyzerDevicePriceCommandHandler(IRepository<AnalyzerDevice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateAnalyzerDevicePriceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        entity.Price = request.Price;

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }


}
