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


public record UpdateAnalyzerDeviceCommand : BaseRecordDto<UpdateAnalyzerDeviceCommand, AnalyzerDevice, int>, IRequest<int>
{
    public int PaymentId { get; set; }
    public string Title { get; set; }
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateAnalyzerDeviceCommand, AnalyzerDevice> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateAnalyzerDeviceCommandHandler : IRequestHandler<UpdateAnalyzerDeviceCommand, int>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IMapper _mapper;
    public UpdateAnalyzerDeviceCommandHandler(IRepository<AnalyzerDevice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateAnalyzerDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.
                            FirstOrDefaultAsync(x => x.Id == request.Id);

        //await _timingValidatorService.CheckTimingOfOrder(entity.Order);

        //var AnalyzerDevice = request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }


}
