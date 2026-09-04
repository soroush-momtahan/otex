using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Domain.Identity.ValueObjects;

public record FullName
{
    public const int MinLenght = 3;
    public const int MaxLenght = 50;
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    
    private FullName() {}

    public static Result<FullName> From(string firstname, string lastname)
    {
        firstname = firstname.Trim();
        lastname = lastname.Trim();
        List<Result<FullName>> results = [];
        switch (firstname.Length)
        {
            case < MinLenght:
                results.Add(Result.Failure<FullName>(IdentityErrors.FirstnameIsTooShort));
                break;
            case > MaxLenght:
                results.Add(Result.Failure<FullName>(IdentityErrors.FirstnameIsTooLong));
                break;
        }

        switch (lastname.Length)
        {
            case < MinLenght:
                results.Add(Result.Failure<FullName>(IdentityErrors.LastnameIsTooShort));
                break;
            case > MaxLenght:
                results.Add(Result.Failure<FullName>(IdentityErrors.LastnameIsTooLong));
                break;
        }

        if (results.Count > 0)
        {
            return Result.Combine(results);
        }

        return new FullName()
        {
            Firstname = firstname,
            Lastname = lastname
        };
    }
}