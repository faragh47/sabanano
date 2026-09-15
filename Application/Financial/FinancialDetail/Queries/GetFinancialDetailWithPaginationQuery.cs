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


public record GetFinancialDetailWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<FinancialDetailBriefListDto>>
{
    public long? FinancialId { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public Expression<Func<FinancialDetail, bool>> GenerateExpression(GetFinancialDetailWithPaginationQuery dto)
    {
        List<Expression<Func<FinancialDetail, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (FinancialId is not null)
        {
            expressions.Add(src => src.FinancialId.Equals(FinancialId));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Contains(Title));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetFinancialDetailWithPaginationQueryHandler : IRequestHandler<GetFinancialDetailWithPaginationQuery, PaginatedList<FinancialDetailBriefListDto>>
{
    private readonly IRepository<FinancialDetail> _repository;
    private readonly IMapper _mapper;

    public GetFinancialDetailWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<FinancialDetail> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<FinancialDetailBriefListDto>> Handle(GetFinancialDetailWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<FinancialDetailBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
