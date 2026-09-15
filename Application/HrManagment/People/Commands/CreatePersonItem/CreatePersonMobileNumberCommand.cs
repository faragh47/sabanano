using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
public record CreatePersonMobileNumberCommand : BaseRecordDto<CreatePersonMobileNumberCommand, PersonMobileNumber, long>,IRequest<long>
{
    public string MobileNumber { get; set; }
    public bool? IsDefault { get; set; }
    public long PersonId { get; set; }

}


public class CreatePersonCommandHandler : IRequestHandler<CreatePersonMobileNumberCommand, long>
{
    private readonly IRepository<PersonMobileNumber> _repository;
    private readonly IMapper _mapper;

    public CreatePersonCommandHandler(IRepository<PersonMobileNumber> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreatePersonMobileNumberCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new PersonCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
