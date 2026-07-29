namespace Otex.BuildingBlocks.Infrastructure.Outbox;

public class OutboxModuleConfig(string moduleName)
{
    public string ModuleName { get; } = moduleName;
}
