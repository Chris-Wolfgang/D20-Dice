using System.Collections.Generic;
using Xunit;
// ReSharper disable RedundantArgumentDefaultValue

namespace Wolfgang.D20.Tests.Unit;

public class DieTests
{
    [Fact]
    public void Can_create_instance()
    {
        // Arrange & Act
        var die = new Die();

        // Assert
        Assert.NotNull(die);
    }



    [Fact]
    public void When_not_specified_SideCount_defaults_to_6()
    {
        // Arrange & Act
        var die = new Die();

        // Assert
        Assert.Equal(6, die.SideCount);
    }



    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(20)]
    public void Can_specify_the_number_of_sides(int sideCount)
    {
        // Arrange & Act
        var die = new Die(sideCount);

        // Assert
        Assert.Equal(sideCount, die.SideCount);
    }



    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void SideCount_less_than_2_throws_ArgumentOutOfRangeException(int sideCount)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Die(sideCount));
    }



    [Fact]
    public void MinValue_is_always_1()
    {
        // Arrange & Act
        var die = new Die(20);

        // Assert
        Assert.Equal(1, die.MinValue);
    }



    [Theory]
    [InlineData(2)]
    [InlineData(6)]
    [InlineData(20)]
    public void MaxValue_equals_SideCount(int sideCount)
    {
        // Arrange & Act
        var die = new Die(sideCount);

        // Assert
        Assert.Equal(sideCount, die.MaxValue);
    }



    [Fact]
    public void Can_roll_die()
    {
        // Arrange
        var die = new Die();

        // Act
        var result = die.Roll();

        // Assert
        Assert.InRange(result, die.MinValue, die.MaxValue);
    }



    [Fact]
    public void Roll_when_d6_produces_values_including_min_and_max()
    {
        // Arrange — roll enough times to statistically guarantee a 1 and a 6
        var die = new Die(6);
        var results = new HashSet<int>();

        // Act
        for (var i = 0; i < 1000; i++)
        {
            results.Add(die.Roll());
        }

        // Assert
        Assert.Contains(1, results);
        Assert.Contains(6, results);
    }



    [Theory]
    [InlineData(2, "d2")]
    [InlineData(6, "d6")]
    [InlineData(20, "d20")]
    public void ToString_returns_value_in_die_notation(int sideCount, string expected)
    {
        // Arrange
        var die = new Die(sideCount);

        // Act
        var actual = die.ToString();

        // Assert
        Assert.Equal(expected, actual);
    }



    [Fact]
    public void Two_dice_with_same_SideCount_are_equal()
    {
        // Arrange
        var die1 = new Die(6);
        var die2 = new Die(6);

        // Act & Assert
        Assert.Equal(die1, die2);
    }



    [Fact]
    public void Two_dice_with_different_SideCount_are_not_equal()
    {
        // Arrange
        var die1 = new Die(6);
        var die2 = new Die(8);

        // Act & Assert
        Assert.NotEqual(die1, die2);
    }



    [Fact]
    public void EqualsDie_Null_ReturnsFalse()
    {
        var die = new Die(6);
        Die? other = null;

#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(die.Equals(other));
#pragma warning restore CA1508 // Avoid dead conditional code
    }



    [Fact]
    public void EqualsDie_SameReference_ReturnsTrue()
    {
        var die = new Die(6);
        Assert.True(die.Equals(die));
    }



    [Fact]
    public void EqualsObject_Null_ReturnsFalse()
    {
        var die = new Die(6);
        object? other = null;
#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(die.Equals(other));
#pragma warning restore CA1508 // Avoid dead conditional code
    }



    [Fact]
    public void EqualsObject_SameReference_ReturnsTrue()
    {
        var die = new Die(6);
        Assert.True(die.Equals((object)die));
    }



    [Fact]
    public void EqualsObject_DifferentType_ReturnsFalse()
    {
        var die = new Die(6);
        var notDie = new { SideCount = 6 };
        Assert.False(die.Equals(notDie));
    }



    [Fact]
    public void EqualsObject_SameValues_ReturnsTrue()
    {
        var die1 = new Die(8);
        var die2 = new Die(8);
        Assert.True(die1.Equals((object)die2));
    }



    [Fact]
    public void Two_dice_with_the_same_SideCount_have_the_same_hash_code()
    {
        var die1 = new Die(6);
        var die2 = new Die(6);
        Assert.Equal(die1.GetHashCode(), die2.GetHashCode());
    }



    [Fact]
    public void Die_can_be_cast_to_IDie()
    {
        IDie die = new Die();
        Assert.NotNull(die);
        Assert.IsType<Die>(die);
    }



    [Fact]
    public void Die_can_be_cast_to_IEquatable_Die()
    {
        IEquatable<Die> die = new Die();
        Assert.NotNull(die);
        Assert.IsType<Die>(die);
    }



}
