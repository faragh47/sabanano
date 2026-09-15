namespace CleanArchitecture.Domain.Events;

public class PersonCreatedEvent : BaseEvent
{
    public PersonCreatedEvent(Person item)
    {
        Item = item;
    }

    public Person Item { get; }
}
