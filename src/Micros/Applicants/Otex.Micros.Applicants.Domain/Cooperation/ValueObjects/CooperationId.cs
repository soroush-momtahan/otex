using Otex.BuildingBlocks.Domain.Enums;
using Otex.BuildingBlocks.Domain.PrefixedGuidTools;

namespace Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

[Prefix(nameof(IdType.Cat))]
public record CooperationId(Guid Value) : PrefixedGuidV3(Value);