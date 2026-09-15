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

public record CreateFinancialDetailCommand : BaseRecordDto<CreateFinancialDetailCommand, FinancialDetail, long>, IRequest<long>
{
    public long FinancialId { get; set; }
    public decimal Price { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

public class CreateFinancialDetailCommandHandler : IRequestHandler<CreateFinancialDetailCommand, long>
{
    private readonly IRepository<FinancialDetail> _repository;
    private readonly IMapper _mapper;

    public CreateFinancialDetailCommandHandler(IRepository<FinancialDetail> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateFinancialDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainFinancialDetail(new ProviderCreatedFinancialDetail(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
