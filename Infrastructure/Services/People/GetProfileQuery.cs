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

public record GetProfileQuery : BaseRecordSearchDto, IRequest<PeopleBriefDto>
{
    public Expression<Func<Person, bool>> GenerateExpression(GetProfileQuery dto)
    {
        List<Expression<Func<Person, bool>>> expressions = new();
        return ExpressionsHelper.AndAll(expressions);
    }
}

public class
    GetProfileQueryHandler : IRequestHandler<GetProfileQuery, PeopleBriefDto>
{
    private readonly IRepository<Person> _repository;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetProfileQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IRepository<Person> repository,
        IRepository<ApplicationUser> userRepository,
        ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _repository = repository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PeopleBriefDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var person = await _repository.TableNoTracking
            .Include(x => x.MobileNumbers)
            .Include(x => x.PeopleAddresses)
            .ThenInclude(x => x.Address)
            .Where(expresion)
            .FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
        var result = _mapper.Map<PeopleBriefDto>(person);
        var user = await _userRepository.TableNoTracking.FirstOrDefaultAsync(x => x.PersonId == result.Id);
        result.Email = user.Email;
        return result;
    }
}