using Otex.BuildingBlocks.Domain.Events;
using Otex.Micros.Identity.Domain.Identity.ValueObjects;

namespace Otex.Micros.Identity.Domain.Identity.Events;

public class UserCreatedEvent(UserId userId) : DomainEvent;