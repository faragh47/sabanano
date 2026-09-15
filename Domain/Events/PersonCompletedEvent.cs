namespace CleanArchitecture.Domain.Events;

public class PersonCompletedEvent : BaseEvent
{
    public PersonCompletedEvent(Person item)
    {
        Item = item;
    }

    public Person Item { get; }
}
