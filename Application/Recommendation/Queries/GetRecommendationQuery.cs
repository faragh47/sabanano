using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security;
using MediatR;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetRecommendationQuery : IRequest<TodosVm>;

public class GetRecommendationQueryHandler : IRequestHandler<GetRecommendationQuery, TodosVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecommendationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodosVm> Handle(GetRecommendationQuery request, CancellationToken cancellationToken)
    {
        return null;
    }
}