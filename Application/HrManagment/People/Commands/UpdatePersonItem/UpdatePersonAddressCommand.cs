using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.People.Commands.UpdateAddress;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdatePeopleAddress;

public record UpdatePeopleAddressCommand : BaseRecordDto<UpdatePeopleAddressCommand, PeopleAddress, long>, IRequest<long>
{
    public string Title { get; set; }
    public UpdateAddressCommand Address { get; set; }
    public long PersonId { get; set; }
    public bool IsDefault { get; set; }

    public override void CustomMappings(IMappingExpression<UpdatePeopleAddressCommand, PeopleAddress> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdatePeopleAddressCommand, long>
{
    private readonly IRepository<PeopleAddress> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<PeopleAddress> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdatePeopleAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var PeopleAddress = request.ToEntity(_mapper, entity);

        PeopleAddress.Address.Id = entity.AddressId;

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }

}