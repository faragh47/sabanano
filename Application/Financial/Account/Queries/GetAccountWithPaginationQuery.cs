using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetAccountWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<AccountBriefListDto>>
{
    public string Title { get; set; }
    public string ShebaCode { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public decimal Debit { get; set; }
    public Expression<Func<Account, bool>> GenerateExpression(GetAccountWithPaginationQuery dto)
    {
        List<Expression<Func<Account, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Contains(Title));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetAccountWithPaginationQueryHandler : IRequestHandler<GetAccountWithPaginationQuery, PaginatedList<AccountBriefListDto>>
{
    private readonly IRepository<Account> _repository;
    private readonly IMapper _mapper;

    public GetAccountWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Account> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<AccountBriefListDto>> Handle(GetAccountWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<AccountBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
