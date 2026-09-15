using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public class CreateComplaintCommandValidator : AbstractValidator<CreateComplaintCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateComplaintCommandValidator(IApplicationDbContext context)
    {
        _context = context;
    }

}
