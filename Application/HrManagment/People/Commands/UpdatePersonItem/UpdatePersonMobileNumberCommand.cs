using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdatePersonMobileNumber;

public record UpdatePersonMobileNumberCommand : BaseRecordDto<UpdatePersonMobileNumberCommand, PersonMobileNumber, long>, IRequest<long>
{
    public string MobileNumber { get; set; }
    public bool? IsDefault { get; set; }
    //  public long FPersonId { get; set; }
    public long PersonId { get; set; }

    public override void CustomMappings(IMappingExpression<UpdatePersonMobileNumberCommand, PersonMobileNumber> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdatePersonMobileNumberCommand, long>
{
    private readonly IRepository<PersonMobileNumber> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<PersonMobileNumber> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdatePersonMobileNumberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var PersonMobileNumber = request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.Id;
    }

}