using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateDiscountUsed;

public record UpdateDiscountUsedCommand : BaseRecordDto<UpdateDiscountUsedCommand, DiscountUsed, long>, IRequest<long>
{
    public long DiscountId { get; set; }
    public long PersonId { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateDiscountUsedCommand, DiscountUsed> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateDiscountUsedCommand, long>
{
    private readonly IRepository<DiscountUsed> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<DiscountUsed> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateDiscountUsedCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var DiscountUsed= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
