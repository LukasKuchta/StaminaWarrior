namespace Backend.Domain.Warriors;

public record Experience(uint Value)
{
    public static readonly Experience Zero = new(0);
}
