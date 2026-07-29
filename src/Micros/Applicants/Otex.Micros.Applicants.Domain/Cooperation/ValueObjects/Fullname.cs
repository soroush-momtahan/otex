using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Applicants.Domain.Cooperation.Errors;

namespace Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

public record Fullname
{
    public const int MinLenght = 3;
    public const int MaxLenght = 50;
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    
    private Fullname() {}

    public static Result<Fullname> From(string firstname, string lastname)
    {
        firstname = firstname.Trim();
        lastname = lastname.Trim();
        List<Result<Fullname>> results = [];
        switch (firstname.Length)
        {
            case < MinLenght:
                results.Add(Result.Failure<Fullname>(CooperationErrors.FirstnameIsTooShort));
                break;
            case > MaxLenght:
                results.Add(Result.Failure<Fullname>(CooperationErrors.FirstnameIsTooLong));
                break;
        }

        switch (lastname.Length)
        {
            case < MinLenght:
                results.Add(Result.Failure<Fullname>(CooperationErrors.LastnameIsTooShort));
                break;
            case > MaxLenght:
                results.Add(Result.Failure<Fullname>(CooperationErrors.LastnameIsTooLong));
                break;
        }

        if (results.Count > 0)
        {
            return Result.Combine(results);
        }

        return new Fullname()
        {
            Firstname = firstname,
            Lastname = lastname
        };
    }
}