using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Common.Exceptions;
using Common;
using CleanArchitecture.Domain.Entities.FinancialAggregate;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateAccountCommand : BaseRecordDto<CreateAccountCommand, Account, long>, IRequest<long>
{
    public string Title { get; set; }
    public string ShebaCode { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public decimal Debit { get; set; }
    public string Description { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, long>
{
    private readonly IRepository<Account> _repository;
    private readonly IMapper _mapper;

    public CreateAccountCommandHandler(IRepository<Account> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainAccount(new ProviderCreatedAccount(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
