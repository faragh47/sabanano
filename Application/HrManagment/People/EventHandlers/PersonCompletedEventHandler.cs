using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.People.EventHandlers;

public class PersonCompletedEventHandler : INotificationHandler<PersonCompletedEvent>
{
    private readonly ILogger<PersonCompletedEventHandler> _logger;

    public PersonCompletedEventHandler(ILogger<PersonCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PersonCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
