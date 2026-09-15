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


public record CreateFinancialCommand : BaseRecordDto<CreateFinancialCommand, Financial, long>, IRequest<long>
{
    public long PaymentId { get; set; }
    public string Title { get; set; }
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
}

public class CreateFinancialCommandHandler : IRequestHandler<CreateFinancialCommand, long>
{
    private readonly IRepository<Financial> _repository;
    private readonly IMapper _mapper;

    public CreateFinancialCommandHandler(IRepository<Financial> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateFinancialCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainFinancial(new ProviderCreatedFinancial(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
