using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
// ReSharper disable RedundantArgumentDefaultValue

namespace Wolfgang.D20.Tests.Unit;

public class DiceTests
{
    [Fact]
    public void Can_create_instance()
    {
        // Arrange & Act
        var dice = new Dice();

        // Assert
        Assert.NotNull(dice);
    }



    [Fact]
    public void When_not_specified_DieCount_defaults_to_1()
    {
        // Arrange & Act
        var dice = new Dice();

        // Assert
        Assert.Equal(1, dice.DieCount);
    }



    [Fact]
    public void DieCount_less_than_1_throws_ArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: -1));
    }



    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    public void Can_specify_the_number_of_dice(int dieCount)
    {
        // Arrange & Act
        var dice = new Dice(dieCount: dieCount);

        // Assert
        Assert.Equal(dieCount, dice.DieCount);
    }



    [Fact]
    public void SideCount_less_than_2_throws_ArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1, sideCount: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1, sideCount: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1, sideCount: -1));
    }



    [Fact]
    public void Convenience_constructor_builds_homogeneous_dice()
    {
        // Arrange & Act
        var dice = new Dice(dieCount: 3, sideCount: 8);

        // Assert
        Assert.Equal(3, dice.DieCount);
        Assert.All(dice, die => Assert.Equal(8, die.SideCount));
    }



    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void Modifier_can_be_positive_zero_or_negative_including_int_boundaries(int modifier)
    {
        var dice = new Dice(modifier: modifier);
        Assert.Equal(modifier, dice.Modifier);
    }



    [Fact]
    public void When_not_specified_Modifier_defaults_to_0()
    {
        // Arrange & Act
        var dice = new Dice();

        // Assert
        Assert.Equal(0, dice.Modifier);
    }



    [Fact]
    public void Modifier_can_be_set()
    {
        // Arrange
        var dice = new Dice(1, 6, 0);

        // Act
        dice.Modifier = 5;

        // Assert
        Assert.Equal(5, dice.Modifier);
    }



    // ---- Collection construction ----

    [Fact]
    public void Constructed_from_sequence_of_dice()
    {
        // Arrange
        var source = new[] { new Die(6), new Die(4) };

        // Act
        var dice = new Dice(source, 3);

        // Assert
        Assert.Equal(2, dice.DieCount);
        Assert.Equal(3, dice.Modifier);
    }



    [Fact]
    public void Constructed_from_null_sequence_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Dice((IEnumerable<Die>)null!));
    }



    [Fact]
    public void Constructed_from_sequence_containing_null_throws_ArgumentException()
    {
        var source = new Die?[] { new Die(6), null };
        Assert.Throws<ArgumentException>(() => new Dice(source!));
    }



    // ---- ICollection<Die> behaviour ----

    [Fact]
    public void Add_appends_a_die()
    {
        var dice = new Dice(1, 6);
        dice.Add(new Die(4));

        Assert.Equal(2, dice.DieCount);
        Assert.Equal(2, dice.Count);
    }



    [Fact]
    public void Add_null_throws_ArgumentNullException()
    {
        var dice = new Dice(1, 6);
        Assert.Throws<ArgumentNullException>(() => dice.Add(null!));
    }



    [Fact]
    public void Remove_removes_a_matching_die_and_returns_true()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });

        var removed = dice.Remove(new Die(4));

        Assert.True(removed);
        Assert.Equal(1, dice.DieCount);
    }



    [Fact]
    public void Remove_when_no_match_returns_false()
    {
        var dice = new Dice(1, 6);
        Assert.False(dice.Remove(new Die(20)));
    }



    [Fact]
    public void Remove_null_returns_false()
    {
        var dice = new Dice(1, 6);
        Assert.False(dice.Remove(null!));
    }



    [Fact]
    public void RemoveAt_removes_the_die_at_the_given_index()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4), new Die(8) });

        dice.RemoveAt(1);

        Assert.Equal(2, dice.DieCount);
        Assert.False(dice.Contains(new Die(4)));
    }



    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void RemoveAt_with_index_out_of_range_throws_ArgumentOutOfRangeException(int index)
    {
        var dice = new Dice(2, 6);
        Assert.Throws<ArgumentOutOfRangeException>(() => dice.RemoveAt(index));
    }



    [Fact]
    public void Clear_removes_all_dice_but_keeps_modifier()
    {
        var dice = new Dice(3, 6, 2);

        dice.Clear();

        Assert.Equal(0, dice.DieCount);
        Assert.Equal(2, dice.Modifier);
    }



    [Fact]
    public void Contains_returns_true_for_matching_die()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });

        Assert.True(dice.Contains(new Die(4)));
        Assert.False(dice.Contains(new Die(20)));
    }



    [Fact]
    public void Contains_null_returns_false()
    {
        var dice = new Dice(1, 6);
        Assert.False(dice.Contains(null!));
    }



    [Fact]
    public void CopyTo_copies_dice_to_array()
    {
        var dice = new Dice(2, 6);
        var array = new Die[2];

        dice.CopyTo(array, 0);

        Assert.All(array, die => Assert.Equal(6, die.SideCount));
    }



    [Fact]
    public void IsReadOnly_is_false()
    {
        var dice = new Dice();
        Assert.False(dice.IsReadOnly);
    }



    [Fact]
    public void Can_enumerate_dice()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });

        var sides = dice.Select(die => die.SideCount).ToList();

        Assert.Equal(new[] { 6, 4 }, sides);
    }



    [Fact]
    public void Can_enumerate_dice_via_non_generic_enumerator()
    {
        var dice = new Dice(2, 6);

        var count = 0;
        foreach (var _ in (IEnumerable)dice)
        {
            count++;
        }

        Assert.Equal(2, count);
    }



    // ---- Min / Max ----

    [Theory]
    [InlineData(1, 6, 0, 1)]     // 1d6 = 1
    [InlineData(2, 8, 3, 5)]     // 2d8+3 = 5
    [InlineData(2, 10, -1, 1)]   // 2d10-1 = 1
    [InlineData(3, 4, -5, -2)]   // 3d4-5 = -2
    public void Dice_properly_shows_MinValue(int dieCount, int sideCount, int modifier, int expectedValue)
    {
        var sut = new Dice(dieCount, sideCount, modifier);
        Assert.Equal(expectedValue, sut.MinValue);
    }



    [Theory]
    [InlineData(1, 6, 0, 6)]     // 1d6 = 6
    [InlineData(2, 8, 3, 19)]    // 2d8+3 = 19
    [InlineData(2, 10, -1, 19)]  // 2d10-1 = 19
    [InlineData(3, 4, -5, 7)]    // 3d4-5 = 7
    public void Dice_properly_shows_MaxValue(int dieCount, int sideCount, int modifier, int expectedValue)
    {
        var sut = new Dice(dieCount, sideCount, modifier);
        Assert.Equal(expectedValue, sut.MaxValue);
    }



    [Fact]
    public void MinValue_and_MaxValue_for_heterogeneous_dice()
    {
        // 2d6 + 1d4 + 3
        var dice = new Dice(new[] { new Die(6), new Die(6), new Die(4) }, 3);

        Assert.Equal(6, dice.MinValue);   // 1 + 1 + 1 + 3
        Assert.Equal(19, dice.MaxValue);  // 6 + 6 + 4 + 3
    }



    [Fact]
    public void MinValue_when_overflow_throws_OverflowException()
    {
        var dice = new Dice(new[] { new Die(6) }, int.MaxValue);
        Assert.Throws<OverflowException>(() => dice.MinValue);
    }



    [Fact]
    public void MaxValue_when_overflow_throws_OverflowException()
    {
        var dice = new Dice(new[] { new Die(int.MaxValue), new Die(int.MaxValue) });
        Assert.Throws<OverflowException>(() => dice.MaxValue);
    }



    // ---- Roll ----

    [Fact]
    public void Can_roll_dice()
    {
        var dice = new Dice();
        var result = dice.Roll();
        Assert.InRange(result, dice.MinValue, dice.MaxValue);
    }



    [Fact]
    public void Roll_of_heterogeneous_dice_is_within_range()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) }, 3);

        for (var i = 0; i < 1000; i++)
        {
            Assert.InRange(dice.Roll(), dice.MinValue, dice.MaxValue);
        }
    }



    // ---- ToString ----

    [Theory]
    [InlineData(1, 6, 0, "1d6")]
    [InlineData(2, 8, 3, "2d8+3")]
    [InlineData(2, 10, -1, "2d10-1")]
    [InlineData(3, 4, -5, "3d4-5")]
    public void ToString_returns_value_in_dice_notation(int dieCount, int sideCount, int modifier, string expectedValue)
    {
        var dice = new Dice(dieCount, sideCount, modifier);
        Assert.Equal(expectedValue, dice.ToString());
    }



    [Fact]
    public void ToString_groups_heterogeneous_dice_in_first_appearance_order_with_modifier_last()
    {
        // Dice order is not significant; groups appear in the order each side count is first seen.
        var dice = new Dice(new[] { new Die(4), new Die(6), new Die(6) }, 3);
        Assert.Equal("1d4+2d6+3", dice.ToString());
    }



    [Fact]
    public void ToString_when_collection_is_empty_renders_modifier_only()
    {
        Assert.Equal(string.Empty, new Dice(Array.Empty<Die>()).ToString());
        Assert.Equal("+3", new Dice(Array.Empty<Die>(), 3).ToString());
        Assert.Equal("-2", new Dice(Array.Empty<Die>(), -2).ToString());
    }



    // ---- Equality ----

    [Fact]
    public void Two_dice_with_same_parameters_are_equal()
    {
        Assert.Equal(new Dice(2, 6, 3), new Dice(2, 6, 3));
    }



    [Fact]
    public void Heterogeneous_dice_are_equal_regardless_of_order()
    {
        var dice1 = new Dice(new[] { new Die(6), new Die(4) }, 3);
        var dice2 = new Dice(new[] { new Die(4), new Die(6) }, 3);

        Assert.Equal(dice1, dice2);
        Assert.Equal(dice1.GetHashCode(), dice2.GetHashCode());
    }



    [Fact]
    public void Two_dice_with_different_DieCount_are_not_equal()
    {
        Assert.NotEqual(new Dice(1, 6, 3), new Dice(2, 6, 3));
    }



    [Fact]
    public void Two_dice_with_different_SideCount_are_not_equal()
    {
        Assert.NotEqual(new Dice(2, 6, 3), new Dice(2, 8, 3));
    }



    [Fact]
    public void Two_dice_with_different_Modifier_are_not_equal()
    {
        Assert.NotEqual(new Dice(2, 6, 3), new Dice(2, 6, 4));
    }



    [Fact]
    public void EqualsDice_Null_ReturnsFalse()
    {
        var dice = new Dice(1, 6, 0);
        Dice? other = null;

#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(dice.Equals(other));
#pragma warning restore CA1508 // Avoid dead conditional code
    }



    [Fact]
    public void EqualsDice_SameReference_ReturnsTrue()
    {
        var dice = new Dice(1, 6, 0);
        Assert.True(dice.Equals(dice));
    }



    [Fact]
    public void EqualsObject_Null_ReturnsFalse()
    {
        var dice = new Dice(1, 6, 0);
        object? other = null;
#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(dice.Equals(other));
#pragma warning restore CA1508 // Avoid dead conditional code
    }



    [Fact]
    public void EqualsObject_SameReference_ReturnsTrue()
    {
        var dice = new Dice(1, 6, 0);
        Assert.True(dice.Equals((object)dice));
    }



    [Fact]
    public void EqualsObject_DifferentType_ReturnsFalse()
    {
        var dice = new Dice(1, 6, 0);
        var notDice = new { DieCount = 1, Modifier = 0 };
        Assert.False(dice.Equals(notDice));
    }



    [Fact]
    public void EqualsObject_SameValues_ReturnsTrue()
    {
        Assert.True(new Dice(2, 8, 1).Equals((object)new Dice(2, 8, 1)));
    }



    [Fact]
    public void Two_Dice_with_the_same_parameters_have_the_same_hash_code()
    {
        Assert.Equal(new Dice(2, 6, 3).GetHashCode(), new Dice(2, 6, 3).GetHashCode());
    }



    [Fact]
    public void Two_Dice_with_different_parameters_have_different_hash_codes()
    {
        Assert.NotEqual(new Dice(1, 8, 2).GetHashCode(), new Dice(2, 6, 3).GetHashCode());
    }



    [Fact]
    public void Dice_can_be_cast_to_IDice()
    {
        IDice dice = new Dice();
        Assert.NotNull(dice);
        Assert.IsType<Dice>(dice);
    }



    [Fact]
    public void Dice_can_be_cast_to_IEquatable_Dice()
    {
        IEquatable<Dice> dice = new Dice();
        Assert.NotNull(dice);
        Assert.IsType<Dice>(dice);
    }



    [Fact]
    public void Dice_can_be_cast_to_ICollection_Die()
    {
        ICollection<Die> dice = new Dice();
        Assert.NotNull(dice);
        Assert.IsType<Dice>(dice);
    }



    // ---- TryParse ----

    [Theory]
    [InlineData("1d6", 1, 0, "1d6")]
    [InlineData("2d8+3", 2, 3, "2d8+3")]
    [InlineData("2d10-1", 2, -1, "2d10-1")]
    [InlineData("2d10-1+2", 2, 1, "2d10+1")]
    [InlineData("2d10+1+2", 2, 3, "2d10+3")]
    [InlineData("2d10+0", 2, 0, "2d10")]
    [InlineData("d10", 1, 0, "1d10")]
    [InlineData("d10+3", 1, 3, "1d10+3")]
    [InlineData("d6-1", 1, -1, "1d6-1")]
    [InlineData("2d6+1d4+3", 3, 3, "2d6+1d4+3")]
    [InlineData("  2d6 + 1d4 + 3 ", 3, 3, "2d6+1d4+3")]
    [InlineData("1d4+2d6", 3, 0, "1d4+2d6")]
    public void TryParse_when_notation_is_valid_returns_new_Dice(string notation, int dieCount, int modifier, string roundTrip)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Value);
        Assert.Equal(dieCount, result.Value.DieCount);
        Assert.Equal(modifier, result.Value.Modifier);
        Assert.Equal(roundTrip, result.Value.ToString());
    }



    [Theory]
    [InlineData("-1d6")]   // negative die count
    [InlineData("1d-1")]
    [InlineData("-2147483649d6")]
    [InlineData("2d6++1d4")]
    [InlineData("garbage")]
    public void TryParse_when_dice_notation_is_invalid_fails_with_error_message(string? notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Invalid dice notation format. Value must be in XdY+Z format.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("")]
    public void TryParse_when_dice_notation_is_null_or_whitespace(string? notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Value cannot be null or empty.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("0d6")]
    public void TryParse_when_dice_notation_dice_count_is_less_than_1_fails_with_error_message(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Die count must be greater than 0.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("1d1")]
    [InlineData("1d0")]
    public void TryParse_when_dice_notation_side_count_is_less_than_2_fails_with_error_message(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Side count must be greater than 1.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("2147483648d6")]
    public void TryParse_when_die_count_exceeds_int_range_returns_failure(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Die count value is out of range.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("1d9999999999")]
    public void TryParse_when_side_count_exceeds_int_range_returns_failure(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Side count value is out of range.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("1d6+12345678901234567890")]
    [InlineData("1d6-12345678901234567890")]
    public void TryParse_when_modifier_value_is_out_of_int_range_returns_failure(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Modifier value is out of range.", result.ErrorMessage);
    }



    [Fact]
    public void TryParse_when_modifier_total_overflows_returns_failure()
    {
        var result = Dice.TryParse("1d6+2147483647+1");

        Assert.True(result.Failed);
        Assert.Equal("Modifier value is out of range.", result.ErrorMessage);
    }



    [Fact]
    public void TryParse_round_trips_via_ToString()
    {
        var parsed = Dice.TryParse("2d6+1d4+3");

        Assert.True(parsed.Succeeded);
        var reparsed = Dice.TryParse(parsed.Value!.ToString());

        Assert.True(reparsed.Succeeded);
        Assert.Equal(parsed.Value, reparsed.Value);
    }
}
