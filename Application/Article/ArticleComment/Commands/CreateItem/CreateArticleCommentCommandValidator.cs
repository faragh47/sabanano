using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Articles;
using CleanArchitecture.Domain.Entities.HrManagment;
using Common;
using Common.Utilities;
using Data.Contracts;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateArticleCommentCommandValidator : AbstractValidator<CreateArticleCommentCommand>
{
    private readonly IRepository<ArticleComment> _repository;

    public CreateArticleCommentCommandValidator(IRepository<ArticleComment> repository)
    {
        _repository = repository;

        RuleFor(v => v.Comment)
            .NotEmpty().WithMessage("نظر اجباری است");

    }

}
