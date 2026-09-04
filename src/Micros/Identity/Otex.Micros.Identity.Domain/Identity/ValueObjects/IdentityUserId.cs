using Otex.BuildingBlocks.Domain.Enums;
using Otex.BuildingBlocks.Domain.PrefixedGuidTools;

namespace Otex.Micros.Identity.Domain.Identity.ValueObjects;

[Prefix(nameof(IdType.Idntt))]
public record IdentityUserId(Guid Value) : PrefixedGuidV3(Value);