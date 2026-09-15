using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetComplaintQuery : IRequest<TodosVm>;

public class GetComplaintQueryHandler : IRequestHandler<GetComplaintQuery, TodosVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetComplaintQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodosVm> Handle(GetComplaintQuery request, CancellationToken cancellationToken)
    {
        return null;
    }
}