using Xunit;
// ReSharper disable RedundantArgumentDefaultValue

namespace Wolfgang.D20.Tests.Unit;

public class AverageRollExtensionsTests
{

    // ----- IDie -----

    [Theory]
    [InlineData(6, 4)]    // avg 3.5 -> 4
    [InlineData(20, 11)]  // avg 10.5 -> 11
    [InlineData(4, 3)]    // avg 2.5 -> 3
    [InlineData(3, 2)]    // avg 2.0 -> 2 (whole number is unaffected)
    [InlineData(2, 2)]    // avg 1.5 -> 2
    public void Die_AverageRoundedUp_rounds_the_midpoint_up(int sideCount, int expected)
    {
        // Arrange
        var die = new Die(sideCount);

        // Act
        var actual = die.AverageRoundedUp();

        // Assert
        Assert.Equal(expected, actual);
    }



    [Theory]
    [InlineData(6, 3)]    // avg 3.5 -> 3
    [InlineData(20, 10)]  // avg 10.5 -> 10
    [InlineData(4, 2)]    // avg 2.5 -> 2
    [InlineData(3, 2)]    // avg 2.0 -> 2 (whole number is unaffected)
    [InlineData(2, 1)]    // avg 1.5 -> 1
    public void Die_AverageRoundedDown_rounds_the_midpoint_down(int sideCount, int expected)
    {
        // Arrange
        var die = new Die(sideCount);

        // Act
        var actual = die.AverageRoundedDown();

        // Assert
        Assert.Equal(expected, actual);
    }



    [Fact]
    public void Die_AverageRoundedUp_null_throws_ArgumentNullException()
    {
        // Arrange
        IDie die = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => die.AverageRoundedUp());
    }



    [Fact]
    public void Die_AverageRoundedDown_null_throws_ArgumentNullException()
    {
        // Arrange
        IDie die = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => die.AverageRoundedDown());
    }



    // ----- IDice -----

    [Theory]
    [InlineData(2, 6, 0, 7)]    // 2d6   min 2  max 12  avg 7.0  -> 7
    [InlineData(2, 6, 1, 8)]    // 2d6+1 min 3  max 13  avg 8.0  -> 8
    [InlineData(1, 6, 0, 4)]    // 1d6   min 1  max 6   avg 3.5  -> 4
    [InlineData(3, 8, 0, 14)]   // 3d8   min 3  max 24  avg 13.5 -> 14
    [InlineData(1, 4, -1, 2)]   // 1d4-1 min 0  max 3   avg 1.5  -> 2
    public void Dice_AverageRoundedUp_rounds_the_midpoint_up(int dieCount, int sideCount, int modifier, int expected)
    {
        // Arrange
        var dice = new Dice(dieCount, sideCount, modifier);

        // Act
        var actual = dice.AverageRoundedUp();

        // Assert
        Assert.Equal(expected, actual);
    }



    [Theory]
    [InlineData(2, 6, 0, 7)]    // 2d6   avg 7.0  -> 7
    [InlineData(2, 6, 1, 8)]    // 2d6+1 avg 8.0  -> 8
    [InlineData(1, 6, 0, 3)]    // 1d6   avg 3.5  -> 3
    [InlineData(3, 8, 0, 13)]   // 3d8   avg 13.5 -> 13
    [InlineData(1, 4, -1, 1)]   // 1d4-1 avg 1.5  -> 1
    public void Dice_AverageRoundedDown_rounds_the_midpoint_down(int dieCount, int sideCount, int modifier, int expected)
    {
        // Arrange
        var dice = new Dice(dieCount, sideCount, modifier);

        // Act
        var actual = dice.AverageRoundedDown();

        // Assert
        Assert.Equal(expected, actual);
    }



    [Fact]
    public void Dice_AverageRoundedUp_accounts_for_heterogeneous_dice()
    {
        // Arrange - 1d6 + 1d4: min 2, max 10, avg 6.0
        var dice = new Dice(new List<Die> { new(6), new(4) });

        // Act & Assert
        Assert.Equal(6, dice.AverageRoundedUp());
    }



    [Fact]
    public void Dice_AverageRoundedUp_null_throws_ArgumentNullException()
    {
        // Arrange
        IDice dice = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dice.AverageRoundedUp());
    }



    [Fact]
    public void Dice_AverageRoundedDown_null_throws_ArgumentNullException()
    {
        // Arrange
        IDice dice = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dice.AverageRoundedDown());
    }

}
