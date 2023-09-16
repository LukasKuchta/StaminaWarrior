namespace Backend.Infrastructure.Serialization;
internal interface ISerializer
{
    string Serialize(object? input);
    object? Deserialize(string input, Type type);
}
