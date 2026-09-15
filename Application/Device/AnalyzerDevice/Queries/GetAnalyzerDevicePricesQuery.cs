using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.PagesDto;
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
using Microsoft.EntityFrameworkCore;


public record GetAnalyzerDevicePricesQuery : BaseRecordSearchDto, IRequest<List<AnalyzeDevicePriceDto>>
{
    public string Name { get; set; }
    public Expression<Func<AnalyzerDevice, bool>> GenerateExpression(GetAnalyzerDevicePricesQuery dto)
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

public class GetAnalyzerDevicePricesQueryHandler : IRequestHandler<GetAnalyzerDevicePricesQuery, List<AnalyzeDevicePriceDto>>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IMapper _mapper;

    public GetAnalyzerDevicePricesQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<AnalyzerDevice> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<AnalyzeDevicePriceDto>> Handle(GetAnalyzerDevicePricesQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.TableNoTracking.Select(x => new AnalyzeDevicePriceDto
        {
            Id = x.Id,
            Price = x.Price,
            Title = x.Name
        }).ToListAsync();

        return result;
    }
}
