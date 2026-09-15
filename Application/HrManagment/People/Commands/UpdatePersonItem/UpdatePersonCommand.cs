using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.People.Commands.UpdatePersonMobileNumber;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdatePerson;

public record UpdatePersonCommand : BaseRecordDto<UpdatePersonCommand, Person, long>, IRequest<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public List<UpdatePersonMobileNumberCommand> MobileNumbers { get; set; }


    public override void CustomMappings(IMappingExpression<UpdatePersonCommand, Person> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdatePersonCommand, long>
{
    private readonly IRepository<Person> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<Person> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var person = request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }
}