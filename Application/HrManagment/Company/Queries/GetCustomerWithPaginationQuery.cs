using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetCompanyWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<CompanyBriefDto>>
{
    public Expression<Func<Company, bool>> GenerateExpression(GetCompanyWithPaginationQuery dto)
    {
        List<Expression<Func<Company, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }
        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetCompanyWithPaginationQueryHandler : IRequestHandler<GetCompanyWithPaginationQuery, PaginatedList<CompanyBriefDto>>
{
    private readonly IRepository<Company> _repository;
    private readonly IMapper _mapper;

    public GetCompanyWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Company> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<CompanyBriefDto>> Handle(GetCompanyWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<CompanyBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
