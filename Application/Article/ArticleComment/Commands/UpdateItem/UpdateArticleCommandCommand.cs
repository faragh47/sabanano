using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Commands.UpdateArticleComment;

public record UpdateArticleCommentCommand : BaseRecordDto<UpdateArticleCommentCommand, ArticleComment, int>, IRequest<int>
{
    public string Comment { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateArticleCommentCommand, ArticleComment> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateArticleCommentCommand, int>
{
    private readonly IRepository<ArticleComment> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<ArticleComment> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateArticleCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var ArticleComment= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
