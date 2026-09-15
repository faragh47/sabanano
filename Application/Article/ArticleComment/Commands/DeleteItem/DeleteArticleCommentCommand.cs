using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.DeleteArticleComment;

public record DeleteArticleCommentCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteArticleCommentCommand, int>
{
    private readonly IRepository<ArticleComment> _ArticleCommentRepository;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context,
        IRepository<ArticleComment> ArticleCommentRepository)
    {
        _ArticleCommentRepository = ArticleCommentRepository;
    }

    public async Task<int> Handle(DeleteArticleCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _ArticleCommentRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ArticleComment), request.Id);
        }

        await _ArticleCommentRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
