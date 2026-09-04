
using Otex.BuildingBlocks.Domain.Objects;
using Otex.Micros.Applicants.Domain.Cooperation.Enums;
using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Domain.Cooperation.Models;

public class Cooperation : Aggregate<CooperationId>
{
    public Fullname Fullname { get; init; }
    public Mobile Mobile { get; init; }
    public Location Location { get; init; }
    public TypeOfActivity TypeOfActivity { get; init; }
    public ReservationDateTime? ReserveDateTime { get; init; }
    public Description? Description { get; init; }
    public IsVerified IsVerified { get; private set; }
    private Cooperation() {}

    private Cooperation(
        Fullname fullname,
        Mobile mobile,
        Location location,
        TypeOfActivity typeOfActivity)
    {
        Fullname = fullname;
        Mobile = mobile;
        Location = location;
        TypeOfActivity = typeOfActivity;
        IsVerified = IsVerified.No;
    }

    public static Cooperation Create(
        Fullname fullname,
        Mobile mobile,
        Location location,
        TypeOfActivity typeOfActivity,
        ReservationDateTime? reserveDateTime,
        Description? description)
    {
        return new Cooperation(fullname, mobile, location, typeOfActivity)
        {
            ReserveDateTime =  reserveDateTime,
            Description = description,
        };
    }

    public void Verify()
    {
        IsVerified = IsVerified.Yes;
    }
}
