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


public record GetAnalyzeDeviceResponseWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<AnalyzeDeviceResponseBriefListDto>>
{
    public Expression<Func<AnalyzeDeviceResponse, bool>> GenerateExpression(GetAnalyzeDeviceResponseWithPaginationQuery dto)
    {
        List<Expression<Func<AnalyzeDeviceResponse, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetAnalyzeDeviceResponseWithPaginationQueryHandler : IRequestHandler<GetAnalyzeDeviceResponseWithPaginationQuery, PaginatedList<AnalyzeDeviceResponseBriefListDto>>
{
    private readonly IRepository<AnalyzeDeviceResponse> _repository;
    private readonly IMapper _mapper;

    public GetAnalyzeDeviceResponseWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<AnalyzeDeviceResponse> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<AnalyzeDeviceResponseBriefListDto>> Handle(GetAnalyzeDeviceResponseWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<AnalyzeDeviceResponseBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
