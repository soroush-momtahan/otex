using Otex.BuildingBlocks.Domain.Objects;
using Otex.Micros.Identity.Domain.Identity.Events;
using Otex.Micros.Identity.Domain.Identity.ValueObjects;

namespace Otex.Micros.Identity.Domain.Identity.Models;

public class User : Aggregate<UserId>
{
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 128;
    public const int MinPasswordUniqueChars = 4;
    public IdentityUserId IdentityUserId { get; private set; }
    public FullName FullName { get; private set; }
    public Mobile Mobile { get; private set; }
    
    private User(){}

    private User(
        IdentityUserId identityUserId,
        FullName fullName, 
        Mobile mobile)
    {
        IdentityUserId = identityUserId;
        FullName = fullName;
        Mobile = mobile;
        
        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public static User Create(
        IdentityUserId identityUserId,
        FullName fullName, 
        Mobile mobile)
    {
        return new User(
            identityUserId,
            fullName, 
            mobile);
    }
}