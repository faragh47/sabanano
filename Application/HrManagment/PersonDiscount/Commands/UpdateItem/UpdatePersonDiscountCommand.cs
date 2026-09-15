using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdatePersonDiscount;

public record UpdatePersonDiscountCommand : BaseRecordDto<UpdatePersonDiscountCommand, PersonDiscount, long>, IRequest<long>
{
    public long PersonId { get; set; }
    public long DiscountId { get; set; }
    public override void CustomMappings(IMappingExpression<UpdatePersonDiscountCommand, PersonDiscount> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdatePersonDiscountCommand, long>
{
    private readonly IRepository<PersonDiscount> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<PersonDiscount> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdatePersonDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var PersonDiscount= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
