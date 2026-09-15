using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
public record CreatePeopleAddressCommand : BaseRecordDto<CreatePeopleAddressCommand, PeopleAddress, long>, IRequest<long>
{
    public long PersonId { get; set; }
    public CreateAddressCommand Address { get; set; }
    public string Title { get; set; }
}


public class CreatePeopleAddressCommandHandler : IRequestHandler<CreatePeopleAddressCommand, long>
{
    private readonly IRepository<PeopleAddress> _repository;
    private readonly IMapper _mapper;

    public CreatePeopleAddressCommandHandler(IRepository<PeopleAddress> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreatePeopleAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new PersonCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
