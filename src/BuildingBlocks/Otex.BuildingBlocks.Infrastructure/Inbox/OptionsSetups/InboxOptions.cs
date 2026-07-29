namespace Otex.BuildingBlocks.Infrastructure.Inbox.OptionsSetups;

public sealed class InboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}
