namespace CleanArchitecture.Domain.Entities;

public class TodoItem : BaseAuditableEntity<int>
{
    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done;

    public TodoList List { get; set; } = null!;
}
