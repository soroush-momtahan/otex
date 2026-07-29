namespace Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

public record IsVerified
{
    public bool Value { get; init; }

    private IsVerified(bool value) => Value = value;

    public static readonly IsVerified Yes = new IsVerified(true);
    public static readonly IsVerified No = new IsVerified(false);
}