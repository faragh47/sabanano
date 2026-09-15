using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetPaymentTypeWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<PaymentTypeBriefListDto>>
{
    public string Title { get; set; }
    public Expression<Func<PaymentType, bool>> GenerateExpression(GetPaymentTypeWithPaginationQuery dto)
    {
        List<Expression<Func<PaymentType, bool>>> expressions = new();
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

public class GetPaymentTypeWithPaginationQueryHandler : IRequestHandler<GetPaymentTypeWithPaginationQuery, PaginatedList<PaymentTypeBriefListDto>>
{
    private readonly IRepository<PaymentType> _repository;
    private readonly IMapper _mapper;

    public GetPaymentTypeWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<PaymentType> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<PaymentTypeBriefListDto>> Handle(GetPaymentTypeWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<PaymentTypeBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
