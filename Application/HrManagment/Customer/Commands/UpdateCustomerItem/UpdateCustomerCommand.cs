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

namespace CleanArchitecture.Application.People.Commands.UpdateCustomer;

public record UpdateCustomerCommand : BaseRecordDto<UpdateCustomerCommand, Customer, long>, IRequest<long>
{
    public string CustomerCode { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateCustomerCommand, Customer> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateCustomerCommand,long>
{
    private readonly IRepository<Customer> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<Customer> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var Customer= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
