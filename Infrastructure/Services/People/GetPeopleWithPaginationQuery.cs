using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetPeopleWithPaginationQuery : BaseRecordSearchDto, IRequest<PaginatedList<PeopleBriefDto>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string NationalId { get; set; }
    public string Birthday { get; set; }
    public int? GenderTypeId { get; set; }
    public int? HomeTownCityId { get; set; }
    public int? CountryId { get; set; }
    public string MobileNumber { get; set; }

    public Expression<Func<Person, bool>> GenerateExpression(GetPeopleWithPaginationQuery dto)
    {
        List<Expression<Func<Person, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (!string.IsNullOrEmpty(FirstName))
        {
            expressions.Add(src => src.FirstName.Contains(FirstName));
        }

        if (!string.IsNullOrEmpty(LastName))
        {
            expressions.Add(src => src.LastName.Contains(LastName));
        }

        if (!string.IsNullOrEmpty(FatherName))
        {
            expressions.Add(src => src.FatherName.Contains(FatherName));
        }

        if (!string.IsNullOrEmpty(NationalId))
        {
            expressions.Add(src => src.NationalId.Equals(NationalId));
        }

        if (!string.IsNullOrEmpty(MobileNumber))
        {
            expressions.Add(src => src.MobileNumbers.Any(x => x.MobileNumber.Equals(MobileNumber)));
        }

        if (GenderTypeId is not null)
            expressions.Add(src => src.GenderTypeId.Equals(GenderTypeId));

        if (HomeTownCityId is not null)
            expressions.Add(src => src.HomeTownCityId.Equals(HomeTownCityId));

        if (CountryId is not null)
            expressions.Add(src => src.HomeTownCityId.Equals(HomeTownCityId));

        var date = Birthday.ToGregorianDate();

        if (date is not null)
            expressions.Add(src => src.Birthday.Equals(Birthday));

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class GetPeopleWithPaginationQueryHandler : IRequestHandler<GetPeopleWithPaginationQuery, PaginatedList<PeopleBriefDto>>
{
    private readonly IRepository<Person> _repository;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;

    public GetPeopleWithPaginationQueryHandler(IApplicationDbContext context,
                                               IMapper mapper,
                                               IRepository<Person> repository,
                                               IRepository<ApplicationUser> userRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<PaginatedList<PeopleBriefDto>> Handle(GetPeopleWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var result = await _repository.TableNoTracking.Where(expresion)
            .ProjectTo<PeopleBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync((int)request.PageNumber, (int)request.RecordsPerPage);

        foreach (var item in result.Items)
        {
            var user = await _userRepository.TableNoTracking.FirstOrDefaultAsync(x => x.PersonId == item.Id);
            item.Email = user.Email;
        }

        return result;
    }
}
