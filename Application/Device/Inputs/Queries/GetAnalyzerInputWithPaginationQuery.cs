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


public record GetAnalyzeDeviceInputWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<AnalyzeDeviceInputBriefListDto>>
{
    public Expression<Func<AnalyzeDeviceInput, bool>> GenerateExpression(GetAnalyzeDeviceInputWithPaginationQuery dto)
    {
        List<Expression<Func<AnalyzeDeviceInput, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetAnalyzeDeviceInputWithPaginationQueryHandler : IRequestHandler<GetAnalyzeDeviceInputWithPaginationQuery, PaginatedList<AnalyzeDeviceInputBriefListDto>>
{
    private readonly IRepository<AnalyzeDeviceInput> _repository;
    private readonly IMapper _mapper;

    public GetAnalyzeDeviceInputWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<AnalyzeDeviceInput> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<AnalyzeDeviceInputBriefListDto>> Handle(GetAnalyzeDeviceInputWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<AnalyzeDeviceInputBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
