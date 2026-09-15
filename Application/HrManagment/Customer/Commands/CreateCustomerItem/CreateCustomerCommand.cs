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

public record CreateCustomerCommand : BaseRecordDto<CreateCustomerCommand, Customer, long>, IRequest<long>
{
    public long PersonId { get; set; }
    public string Sheba { get; set; }
    public string Title { get; set; }
    public string CustomerCode { get; set; }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, long>
{
    private readonly IRepository<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateCustomerCommandHandler(IRepository<Customer> repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<long> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        CreateAccountCommand CreateAccountCommand = new CreateAccountCommand()
        {
            Title = request.Title,
            ShebaCode=request.Sheba,
            Balance = 0,
            Credit = 0,
            Debit = 0,
            Description = request.CustomerCode
        };

        var result=await _mediator.Send(CreateAccountCommand);

        entity.AccountId = result;

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new CustomerCreatedEvent(entity));


        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
