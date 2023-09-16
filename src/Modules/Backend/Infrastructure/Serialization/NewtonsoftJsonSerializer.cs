using System.Reflection;
using Backend.Infrastructure.Messaging.InternalCommands;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Backend.Infrastructure.Serialization;
internal class NewtonsoftJsonSerializer : ISerializer
{
    public object? Deserialize(string input, Type type)
    {
        return JsonConvert.DeserializeObject(input, type);
    }

    public string Serialize(object? input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        return JsonConvert.SerializeObject(input, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver(),
        });
    }

    private class AllPropertiesContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = type.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance)
                .Select(p => CreateProperty(p, memberSerialization))
                .ToList();

            properties.ForEach(p =>
            {
                p.Writable = true;
                p.Readable = true;
            });

            return properties;
        }
    }



}
