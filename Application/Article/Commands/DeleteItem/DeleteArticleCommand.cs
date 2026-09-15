using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Entities.Articles;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record DeleteArticleCommand : IRequest<int>
{
    public int Id { get; set; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteArticleCommand,int>
{
    private readonly IRepository<Article> _ArticleRepository;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context,
        IRepository<Article> ArticleRepository)
    {
        _ArticleRepository = ArticleRepository;
    }

    public async Task<int> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _ArticleRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }

        await _ArticleRepository.DeleteAsync(entity, cancellationToken);
        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));
        //await _context.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
