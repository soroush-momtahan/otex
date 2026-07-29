using Newtonsoft.Json;

namespace Otex.BuildingBlocks.Infrastructure.Serialization;

public static class SerializerSettings
{
    public static readonly JsonSerializerSettings Instance = new()
    {        TypeNameHandling = TypeNameHandling.All,  
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
        Converters = new List<JsonConverter>
        {
            new PrefixedGuidJsonConverter()
        }
    };  
}
