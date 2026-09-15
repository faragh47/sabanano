using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.HrManagment;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public record CreateRecommendationCommand : BaseRecordDto<CreateRecommendationCommand, Recommendation, int>, IRequest<int>
{
    public string Description { get; set; }
}

public class CreateRecommendationCommandCommandHandler : IRequestHandler<CreateRecommendationCommand, int>
{
    private readonly IRepository<Recommendation> _repository;
    private readonly IMapper _mapper;

    public CreateRecommendationCommandCommandHandler(IRepository<Recommendation> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateRecommendationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}