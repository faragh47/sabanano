using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Email;
using CleanArchitecture.Domain.Entities.HrManagment;


using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;


public record GetEmailDiscountWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<EmailDiscountListDto>>
{
    public string Email { get; set; }
    public Expression<Func<EmailDiscount, bool>> GenerateExpression(GetEmailDiscountWithPaginationQuery dto)
    {
        List<Expression<Func<EmailDiscount, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Email))
        {
            expressions.Add(src => src.Email.Contains(Email));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetEmailDiscountWithPaginationQueryHandler : IRequestHandler<GetEmailDiscountWithPaginationQuery, PaginatedList<EmailDiscountListDto>>
{
    private readonly IRepository<EmailDiscount> _repository;
    private readonly IMapper _mapper;

    public GetEmailDiscountWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<EmailDiscount> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<EmailDiscountListDto>> Handle(GetEmailDiscountWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result= await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<EmailDiscountListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
