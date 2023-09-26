namespace Backend.Infrastructure.DistributedCaching;
public sealed class RedisOptions
{
    public string Endpoint { get; init; }
    public int Port { get; init; }

    public string GetConnectionString()
    {
        return $"{Endpoint}:{Port}";
    }
}
