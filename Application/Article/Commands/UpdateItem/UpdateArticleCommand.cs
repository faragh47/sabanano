using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Entities.Articles;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;


public record UpdateArticleCommand : BaseRecordDto<UpdateArticleCommand, Article, int>, IRequest<int>
{
    public string ArticleCode { get; set; }
    public override void CustomMappings(IMappingExpression<UpdateArticleCommand, Article> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand,int>
{
    private readonly IRepository<Article> _repository;
    private readonly IMapper _mapper;

    public UpdateArticleCommandHandler(IRepository<Article> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

        var article= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
