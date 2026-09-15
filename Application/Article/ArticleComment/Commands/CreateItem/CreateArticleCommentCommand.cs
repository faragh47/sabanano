using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateArticleCommentCommand : BaseRecordDto<CreateArticleCommentCommand, ArticleComment, int>, IRequest<int>
{
    public int ArticleId { get; set; }
    public string Comment { get; set; }
    public string IssuerName { get; set; }
    public string IssuerEmail { get; set; }
}

public class CreateArticleCommentCommandHandler : IRequestHandler<CreateArticleCommentCommand, int>
{
    private readonly IRepository<ArticleComment> _repository;
    private readonly IMapper _mapper;

    public CreateArticleCommentCommandHandler(IRepository<ArticleComment> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateArticleCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new ArticleCommentCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
