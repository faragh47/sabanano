using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;


public record GetAnalyzerDeviceSampleWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<AnalyzerDeviceSampleBriefListDto>>
{
    public Expression<Func<AnalyzerDeviceSample, bool>> GenerateExpression(GetAnalyzerDeviceSampleWithPaginationQuery dto)
    {
        List<Expression<Func<AnalyzerDeviceSample, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetAnalyzerDeviceSampleWithPaginationQueryHandler : IRequestHandler<GetAnalyzerDeviceSampleWithPaginationQuery, PaginatedList<AnalyzerDeviceSampleBriefListDto>>
{
    private readonly IRepository<AnalyzerDeviceSample> _repository;
    private readonly IMapper _mapper;

    public GetAnalyzerDeviceSampleWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<AnalyzerDeviceSample> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<AnalyzerDeviceSampleBriefListDto>> Handle(GetAnalyzerDeviceSampleWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<AnalyzerDeviceSampleBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
