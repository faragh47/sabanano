using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetPeopleAddressWithPaginationQuery : BaseRecordSearchDto, IRequest<List<PeopleAddressBriefDto>>
{
    public long? PersonId { get; set; }

    public Expression<Func<PeopleAddress, bool>> GenerateExpression(GetPeopleAddressWithPaginationQuery dto)
    {
        List<Expression<Func<PeopleAddress, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (PersonId > 0)
        {
            expressions.Add(src => src.PersonId.Equals(PersonId));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetPeopleAddressWithPaginationQueryHandler : IRequestHandler<GetPeopleAddressWithPaginationQuery, List<PeopleAddressBriefDto>>
{
    private readonly IRepository<PeopleAddress> _repository;
    private readonly IMapper _mapper;

    public GetPeopleAddressWithPaginationQueryHandler(IApplicationDbContext context,
                                               IMapper mapper,
                                               IRepository<PeopleAddress> repository
                                          )
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<PeopleAddressBriefDto>> Handle(GetPeopleAddressWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result = await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<PeopleAddressBriefDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return result;
    }
}
