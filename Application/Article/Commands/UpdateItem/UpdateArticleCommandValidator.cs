
using FluentValidation;

namespace CleanArchitecture.Application.People.Commands.UpdateArticle;

public class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
{
    public UpdateArticleCommandValidator()
    {
    }
}
