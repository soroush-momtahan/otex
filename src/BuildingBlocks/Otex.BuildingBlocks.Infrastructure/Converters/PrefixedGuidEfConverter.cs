using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Otex.BuildingBlocks.Domain.PrefixedGuidTools;

namespace Otex.BuildingBlocks.Infrastructure.Converters;

public class PrefixedGuidEfConverter<TId>() : ValueConverter<TId, Guid>(
    id => id.Value,
    valueFromDb => (TId)Activator.CreateInstance(typeof(TId), valueFromDb)!)
    where TId : PrefixedGuidV3;

// builder.Properties<ProductId>()
#pragma warning disable S125
//     .HaveConversion<PrefixedGuidEfConverter<ProductId>>();
#pragma warning restore S125
