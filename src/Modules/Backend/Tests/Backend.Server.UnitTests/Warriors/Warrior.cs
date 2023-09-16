namespace Backend.UnitTests.Warriors;

public class Warrior
{
    private readonly User _user = User.Create(UserId.New(), "Jakopo", "Elegrius", "jakopo@ele.co");

    [Fact]
    public void Create_Warrior_Should_Success()
    {

    }

    [Fact]
    public void Create_Warrior_Should_Failed_Max_Limit_Reached()
    {

    }


    [Fact]
    public void Create_Warrior_Should_Failed_By_Invalid_Name()
    {

    }

    [Fact]
    public void Create_Warrior_Should_Failed_By_Null_Name()
    {

    }
}