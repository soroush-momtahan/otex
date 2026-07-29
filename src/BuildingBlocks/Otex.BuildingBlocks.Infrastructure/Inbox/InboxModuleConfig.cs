namespace Otex.BuildingBlocks.Infrastructure.Inbox;

public class InboxModuleConfig(string moduleName)
{
    public string ModuleName { get; } = moduleName;
}
