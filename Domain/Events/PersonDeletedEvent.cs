namespace CleanArchitecture.Domain.Events;

public class PersonDeletedEvent : BaseEvent
{
    public PersonDeletedEvent(Person item)
    {
        Item = item;
    }

    public Person Item { get; }
}
