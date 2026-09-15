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

namespace CleanArchitecture.Application.People.Commands.UpdateDiscountIncrease;

public record UpdateDiscountIncreaseCommand : BaseRecordDto<UpdateDiscountIncreaseCommand, DiscountIncrease, long>, IRequest<long>
{
    public long DistcountId { get; set; }
    public int Percent { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateDiscountIncreaseCommand, DiscountIncrease> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateDiscountIncreaseCommand, long>
{
    private readonly IRepository<DiscountIncrease> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<DiscountIncrease> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateDiscountIncreaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var DiscountIncrease= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
