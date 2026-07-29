using Otex.BuildingBlocks.Domain.Events;
using Otex.BuildingBlocks.Domain.PrefixedGuidTools;

namespace Otex.BuildingBlocks.Domain.Objects;

public class Aggregate<T> : Entity<T>, IAggregate where T : PrefixedGuidV3
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void Clear()
    {
        _domainEvents.Clear();
    }
}

public class Aggregate : Entity, IAggregate
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void Clear()
    {
        _domainEvents.Clear();
    }
}

public interface IAggregate
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void Clear();
}

