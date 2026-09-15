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

namespace CleanArchitecture.Application.People.Commands.UpdateAddress;

public record UpdateAddressCommand : BaseRecordDto<UpdateAddressCommand, Address, long>, IRequest<long>
{
    public int? CityId { get; set; }
    public string FullAddress { get; set; }
    public string PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string PhoneNumber { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateAddressCommand, Address> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateAddressCommand,long>
{
    private readonly IRepository<Address> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<Address> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        var Address= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(Address,cancellationToken);

        return Address.Id;
    }

   
}
