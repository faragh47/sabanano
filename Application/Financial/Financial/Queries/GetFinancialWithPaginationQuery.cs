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


public record GetFinancialWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<FinancialBriefListDto>>
{
    public long? PaymentId { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public Expression<Func<Financial, bool>> GenerateExpression(GetFinancialWithPaginationQuery dto)
    {
        List<Expression<Func<Financial, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (PaymentId is not null)
        {
            expressions.Add(src => src.PaymentId.Equals(PaymentId));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Contains(Title));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetFinancialWithPaginationQueryHandler : IRequestHandler<GetFinancialWithPaginationQuery, PaginatedList<FinancialBriefListDto>>
{
    private readonly IRepository<Financial> _repository;
    private readonly IMapper _mapper;

    public GetFinancialWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Financial> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<FinancialBriefListDto>> Handle(GetFinancialWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<FinancialBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
