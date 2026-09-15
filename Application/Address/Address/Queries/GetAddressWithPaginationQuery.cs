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

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetAddressWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<AddressBriefDto>>
{
    public int? CityId { get; set; }
    public string FullAddress { get; set; }
    public string PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public Expression<Func<Address, bool>> GenerateExpression(GetAddressWithPaginationQuery dto)
    {
        List<Expression<Func<Address, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(PostalCode))
        {
            expressions.Add(src => src.PostalCode.Equals(PostalCode));
        }

        if (!string.IsNullOrEmpty(FullAddress))
        {
            expressions.Add(src => src.FullAddress.Contains(FullAddress));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetAddressWithPaginationQueryHandler : IRequestHandler<GetAddressWithPaginationQuery, PaginatedList<AddressBriefDto>>
{
    private readonly IRepository<Address> _repository;
    private readonly IMapper _mapper;

    public GetAddressWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Address> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<AddressBriefDto>> Handle(GetAddressWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<AddressBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
