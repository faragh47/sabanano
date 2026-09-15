using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateAddressCommand : BaseRecordDto<CreateAddressCommand, Address, long>, IRequest<long>
{
    public int? CityId { get; set; }
    public string FullAddress { get; set; }
    public string PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string PhoneNumber { get; set; }
    public int Floor { get; set; }
    public int Unit { get; set; }
    public int Number { get; set; }
}

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, long>
{
    private readonly IRepository<Address> _repository;
    private readonly IMapper _mapper;

    public CreateAddressCommandHandler(IRepository<Address> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
            var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new AddressCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
