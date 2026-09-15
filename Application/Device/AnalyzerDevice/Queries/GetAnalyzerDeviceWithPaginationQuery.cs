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


public record GetAnalyzerDeviceWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<AnalyzerDeviceListDto>>
{
    public string Name { get; set; }
    public Expression<Func<AnalyzerDevice, bool>> GenerateExpression(GetAnalyzerDeviceWithPaginationQuery dto)
    {
        List<Expression<Func<AnalyzerDevice, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Name))
        {
            expressions.Add(src => src.Name.Equals(Name));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetAnalyzerDeviceWithPaginationQueryHandler : IRequestHandler<GetAnalyzerDeviceWithPaginationQuery, PaginatedList<AnalyzerDeviceListDto>>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IMapper _mapper;

    public GetAnalyzerDeviceWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<AnalyzerDevice> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<AnalyzerDeviceListDto>> Handle(GetAnalyzerDeviceWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<AnalyzerDeviceListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
