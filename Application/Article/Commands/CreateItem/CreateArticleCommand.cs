using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Entities.Articles;
using Common.Utilities;
using Data.Contracts;
using MediatR;


public record CreateArticleCommand : BaseRecordDto<CreateArticleCommand, Article, int>, IRequest<int>
{
    public string Name { get; set; }
}

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, int>
{
    private readonly IRepository<Article> _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateArticleCommandHandler(IRepository<Article> repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        return entity.Id;
    }
}
