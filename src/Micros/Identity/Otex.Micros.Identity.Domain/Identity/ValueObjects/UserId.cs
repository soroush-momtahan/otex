using Otex.BuildingBlocks.Domain.Enums;
using Otex.BuildingBlocks.Domain.PrefixedGuidTools;

namespace Otex.Micros.Identity.Domain.Identity.ValueObjects;

[Prefix(nameof(IdType.Usr))]
public record UserId(Guid Value) : PrefixedGuidV3(Value);