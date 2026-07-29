
using Otex.BuildingBlocks.Domain.Objects;
using Otex.Micros.Applicants.Domain.Cooperation.Enums;
using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Domain.Cooperation.Models;

public class Cooperation : Aggregate<CooperationId>
{
    public required Fullname Fullname { get; init; }
    public required Mobile Mobile { get; init; }
    public required TypeOfActivity TypeOfActivity { get; init; }
    public Description? Description { get; init; }
    public IsVerified IsVerified { get; private set; } = IsVerified.No;
    private Cooperation() {}

    public static Cooperation Create(
        Fullname fullname,
        Mobile mobile,
        TypeOfActivity typeOfActivity,
        Description description)
    {
        return new Cooperation
        {
            Fullname = fullname,
            Mobile = mobile,
            TypeOfActivity = typeOfActivity,
            Description = description
        };
    }

    public void Verify()
    {
        IsVerified = IsVerified.Yes;
    }
}