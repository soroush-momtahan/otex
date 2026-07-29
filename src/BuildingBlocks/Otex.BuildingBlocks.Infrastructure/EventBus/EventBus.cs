using MassTransit;
using Otex.BuildingBlocks.Application.EventBus;

namespace Otex.BuildingBlocks.Infrastructure.EventBus;

internal sealed class EventBus(IBus bus) : IEventBus
{

    public async Task PublishAsync<T>(
        T integrationEvent, 
        CancellationToken cancellationToken = default) 
        where T : IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
