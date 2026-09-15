using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;


public record GetDiscountWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<DiscountBriefListDto>>
{
    public string ExpirationDate { get; set; }
    public int? Count { get; set; }
    public int DistcountTypeId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public string Description { get; set; }

    public Expression<Func<Discount, bool>> GenerateExpression(GetDiscountWithPaginationQuery dto)
    {
        List<Expression<Func<Discount, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(Title))
        {
            expressions.Add(src => src.Title.Contains(Title));
        }

        if (!string.IsNullOrEmpty(LatinTitle))
        {
            expressions.Add(src => src.LatinTitle.Contains(LatinTitle));
        }

        if (!string.IsNullOrEmpty(Code))
        {
            expressions.Add(src => src.Code.Contains(Code));
        }

        var expirationDate = ExpirationDate.ToGregorianDate();

        if (expirationDate is not null)
        {
            expressions.Add(src => src.ExpirationDate == expirationDate);
        }


        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetDiscountWithPaginationQueryHandler : IRequestHandler<GetDiscountWithPaginationQuery, PaginatedList<DiscountBriefListDto>>
{
    private readonly IRepository<Discount> _repository;
    private readonly IMapper _mapper;

    public GetDiscountWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Discount> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<DiscountBriefListDto>> Handle(GetDiscountWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result = await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<DiscountBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        return result;
    }
}
