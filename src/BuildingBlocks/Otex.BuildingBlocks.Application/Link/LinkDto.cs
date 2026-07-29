namespace Otex.BuildingBlocks.Application.Link;

public record LinkDto
{
    public string Rel { get; init; }
    public string Href { get; init; }
    public string Method { get; init; }
    public bool Enabled { get; init; }
}
