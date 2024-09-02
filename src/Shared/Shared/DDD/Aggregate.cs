namespace Shared.DDD;

// The Aggregate class is an Entity + adds the concept of domain events. 
// It represents an entity that also manages a list of domain events (DomainEvents), which are significant occurrences within the domain that need to be 
//It includes methods to add domain events (AddDomainEvent) and clear them after they’ve been handled (ClearDomainArray).

//Domain events are a way to capture and communicate that something important has happened in the domain (e.g., an order was placed, a payment was made).
//These events are stored in the aggregate’s DomainEvents list and can be dispatched to other parts of the system, such as for logging, integration with other systems, or triggering workflows.

#region Abstraction
public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}
#endregion

public abstract class Aggregate<TID> : Entity<TID>, IAggregate<TID>
{
    public readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = [.. _domainEvents];
        _domainEvents.Clear();
        return dequeuedEvents;
    }
}