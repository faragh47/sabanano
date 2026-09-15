using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public class CreateRecommendationCommandValidator : AbstractValidator<CreateRecommendationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateRecommendationCommandValidator(IApplicationDbContext context)
    {
        _context = context;
    }

}
