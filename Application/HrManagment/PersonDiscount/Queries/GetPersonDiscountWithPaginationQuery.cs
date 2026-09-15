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

public record GetPersonDiscountWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<PersonDiscountBriefListDto>>
{

    public long? PersonId { get; set; }
    public long? DiscountId { get; set; }
    public Expression<Func<PersonDiscount, bool>> GenerateExpression(GetPersonDiscountWithPaginationQuery dto)
    {
        List<Expression<Func<PersonDiscount, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }
        if (PersonId is not null)
        {
            expressions.Add(src => src.PersonId.Equals(PersonId));
        }
        if (DiscountId is not null)
        {
            expressions.Add(src => src.DiscountId.Equals(DiscountId));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetPersonDiscountWithPaginationQueryHandler : IRequestHandler<GetPersonDiscountWithPaginationQuery, PaginatedList<PersonDiscountBriefListDto>>
{
    private readonly IRepository<PersonDiscount> _repository;
    private readonly IMapper _mapper;

    public GetPersonDiscountWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<PersonDiscount> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginatedList<PersonDiscountBriefListDto>> Handle(GetPersonDiscountWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);

        var amounts = new List<DiscountAmount>();

        var result= await _repository.TableNoTracking
            .Include(x=>x.Discount)
            .Where(expresion)
            .ProjectTo<PersonDiscountBriefListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        foreach (var item in result.Items)
        {
            for (int i = 0; i < item.Count; i++)
            {
                int increase = 0;

                if (i == 0)
                {
                    increase = 0;
                }
                else if (i > item.Increases.Count()-1) 
                {
                    increase = item.Increases[item.Increases.Count() - 1].Percent;
                }
                else
                {
                    increase = item.Increases[i].Percent;
                }
                var amount = new DiscountAmount()
                {
                    Amount = item.Amount,
                    Max = item.Max,
                    Percent = item.Percent+increase
                };
                amounts.Add(amount);
            }

            item.Amounts.AddRange(amounts);
        }

        return result;
    }
}
