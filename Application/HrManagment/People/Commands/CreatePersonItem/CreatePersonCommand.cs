using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreatePersonCommand : BaseRecordDto<CreatePersonCommand, Person, long>, IRequest<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public string FatherName { get; set; }
    public string NationalId { get; set; }
    public string Birthday { get; set; }
    public int? GenderTypeId { get; set; }
    public int? HomeTownCityId { get; set; }
    public int? CountryId { get; set; }
    public List<CreatePersonMobileNumberCommand> MobileNumbers { get; set; }
    public List<CreatePeopleAddressCommand> PeopleAddresses { get; set; }

    public override void CustomMappings(IMappingExpression<CreatePersonCommand, Person> mappingExpression)
    {
        mappingExpression.ForMember(dest => dest.Birthday, conf => conf.MapFrom(src => src.Birthday.ToGregorianDate()));
    }
}

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, long>
{
    private readonly IRepository<Person> _repository;
    private readonly IMapper _mapper;

    public CreatePersonCommandHandler(IRepository<Person> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new PersonCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
