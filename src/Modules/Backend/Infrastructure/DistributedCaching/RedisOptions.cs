namespace Backend.Infrastructure.DistributedCaching;
internal sealed class RedisOptions
{
    public string Endpoint { get; init; }
    public int Port { get; init; }

    public string GetConnectionString()
    {
        return $"{Endpoint}:{Port}";
    }
}
