using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Domain.Identity.ValueObjects;

public record Mobile
{
    public const int ValidLenght = 11;
    public string Value { get; init; }
    private Mobile() {}

    public static Result<Mobile> From(string mobile)
    {
        mobile = mobile.Trim();
        if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != ValidLenght)
        {
            return Result.Failure<Mobile>(IdentityErrors.MobileNotValid);
        }

        return new Mobile()
        {
            Value = mobile
        };
    }
}