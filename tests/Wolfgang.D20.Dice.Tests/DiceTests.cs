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
    public void Constructor_when_dieCount_is_not_specified_defaults_to_1()
    {
        // Arrange & Act
        var dice = new Dice();

        // Assert
        Assert.Equal(1, dice.DieCount);
    }



    [Fact]
    public void Constructor_when_dieCount_is_less_than_1_throws_ArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: -1));
    }



    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    public void Constructor_when_dieCount_is_given_creates_that_many_dice(int dieCount)
    {
        // Arrange & Act
        var dice = new Dice(dieCount: dieCount);

        // Assert
        Assert.Equal(dieCount, dice.DieCount);
    }



    // ---- Die-count cap (ADR-0009) ----

    [Fact]
    public void MaxDieCount_is_10000()
    {
        Assert.Equal(10_000, Dice.MaxDieCount);
    }



    [Fact]
    public void Constructor_when_dieCount_equals_MaxDieCount_creates_the_pool()
    {
        var dice = new Dice(dieCount: Dice.MaxDieCount, sideCount: 6);

        Assert.Equal(Dice.MaxDieCount, dice.DieCount);
    }



    [Fact]
    public void Constructor_when_dieCount_exceeds_MaxDieCount_throws_ArgumentOutOfRangeException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: Dice.MaxDieCount + 1, sideCount: 6));

        Assert.Equal("dieCount", exception.ParamName);
        Assert.Equal(Dice.MaxDieCount + 1, exception.ActualValue);
        Assert.StartsWith("Die count must be at most 10000.", exception.Message, StringComparison.Ordinal);
    }



    [Fact]
    public void Constructor_when_sequence_has_MaxDieCount_dice_creates_the_pool()
    {
        var dice = new Dice(Enumerable.Repeat(new Die(6), Dice.MaxDieCount));

        Assert.Equal(Dice.MaxDieCount, dice.DieCount);
    }



    [Fact]
    public void Constructor_when_sequence_exceeds_MaxDieCount_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => new Dice(Enumerable.Repeat(new Die(6), Dice.MaxDieCount + 1)));

        Assert.Equal("dice", exception.ParamName);
        Assert.StartsWith("Sequence cannot contain more than 10000 dice.", exception.Message, StringComparison.Ordinal);
    }



    [Fact]
    public void Constructor_when_sequence_is_infinite_throws_instead_of_hanging()
    {
        // The cap is enforced per element, so an endless sequence is rejected at the limit rather than
        // enumerated forever.
        static IEnumerable<Die> Forever()
        {
            while (true)
            {
                yield return new Die(6);
            }
        }

        Assert.Throws<ArgumentException>(() => new Dice(Forever()));
    }



    [Fact]
    public void WithDie_when_pool_already_holds_MaxDieCount_dice_throws_InvalidOperationException()
    {
        var full = new Dice(dieCount: Dice.MaxDieCount, sideCount: 6);

        var exception = Assert.Throws<InvalidOperationException>(() => full.WithDie(new Die(4)));

        Assert.Equal("Sequence cannot contain more than 10000 dice.", exception.Message);
    }



    [Fact]
    public void WithDie_when_pool_holds_one_less_than_MaxDieCount_adds_the_die()
    {
        var almostFull = new Dice(dieCount: Dice.MaxDieCount - 1, sideCount: 6);

        var full = almostFull.WithDie(new Die(4));

        Assert.Equal(Dice.MaxDieCount, full.DieCount);
    }



    [Fact]
    public void Constructor_when_sideCount_is_less_than_2_throws_ArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1, sideCount: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1, sideCount: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 1, sideCount: -1));
    }



    [Fact]
    public void Constructor_when_sideCount_is_less_than_2_reports_sideCount_as_the_parameter_name()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(dieCount: 3, sideCount: 1));

        Assert.Equal("sideCount", exception.ParamName);
    }



    [Fact]
    public void Constructor_when_sideCount_is_2_creates_two_sided_dice()
    {
        var dice = new Dice(dieCount: 1, sideCount: 2);

        Assert.Equal(2, dice.MaxValue);
    }



    [Fact]
    public void Constructor_when_given_dieCount_and_sideCount_builds_homogeneous_dice()
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
    public void Constructor_when_modifier_is_any_int_including_the_boundaries_stores_it(int modifier)
    {
        var dice = new Dice(modifier: modifier);
        Assert.Equal(modifier, dice.Modifier);
    }



    [Fact]
    public void Constructor_when_modifier_is_not_specified_defaults_to_0()
    {
        // Arrange & Act
        var dice = new Dice();

        // Assert
        Assert.Equal(0, dice.Modifier);
    }



    // ---- Collection construction ----

    [Fact]
    public void Constructor_when_given_a_sequence_of_dice_copies_them_and_the_modifier()
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
    public void Constructor_when_sequence_is_null_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Dice(null!));
    }



    [Fact]
    public void Constructor_when_sequence_contains_null_throws_ArgumentException()
    {
        // Explicit `Die?[]` is required because arrays are invariant: an inferred
        // `new[] { new Die(6), null }` would bind to `Die[]` under nullable-enabled
        // and can't legally hold a null element.
        // ReSharper disable once RedundantExplicitArrayCreation
        var source = new Die?[] { new Die(6), null };
        var exception = Assert.Throws<ArgumentException>(() => new Dice(source!));

        Assert.Equal("dice", exception.ParamName);
        Assert.StartsWith("Sequence cannot contain a null die.", exception.Message, StringComparison.Ordinal);
    }



    // ---- Immutable "with" builders ----

    [Fact]
    public void WithDie_returns_a_new_pool_with_the_die_added()
    {
        var dice = new Dice(1, 6, 2);

        var result = dice.WithDie(new Die(4));

        Assert.Equal(2, result.DieCount);
        Assert.Equal(2, result.Modifier);
        Assert.Contains(result, die => die.SideCount == 4);
    }



    [Fact]
    public void WithDie_does_not_modify_the_original()
    {
        var dice = new Dice(1, 6);

        dice.WithDie(new Die(4));

        Assert.Equal(1, dice.DieCount);
    }



    [Fact]
    public void WithDie_null_throws_ArgumentNullException()
    {
        var dice = new Dice(1, 6);
        Assert.Throws<ArgumentNullException>(() => dice.WithDie(null!));
    }



    [Fact]
    public void Without_returns_a_new_pool_with_the_first_match_removed()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });

        var result = dice.Without(new Die(4));

        Assert.Equal(1, result.DieCount);
        Assert.DoesNotContain(result, die => die.SideCount == 4);
    }



    [Fact]
    public void Without_when_pool_has_duplicates_removes_only_the_first_match()
    {
        // [d6, d4] cannot tell "remove the first match" from "remove every match"; two d6 can.
        var dice = new Dice(new[] { new Die(6), new Die(6), new Die(4) }, 1);

        var result = dice.Without(new Die(6));

        Assert.Equal(2, result.DieCount);
        Assert.Equal(1, result.Count(die => die.SideCount == 6));
        Assert.Contains(result, die => die.SideCount == 4);
        Assert.Equal(1, result.Modifier);
    }



    [Fact]
    public void Without_when_no_match_returns_an_equal_copy()
    {
        var dice = new Dice(1, 6);

        var result = dice.Without(new Die(20));

        Assert.Equal(dice, result);
    }



    [Fact]
    public void Without_does_not_modify_the_original()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });

        dice.Without(new Die(4));

        Assert.Equal(2, dice.DieCount);
    }



    [Fact]
    public void Without_null_throws_ArgumentNullException()
    {
        var dice = new Dice(1, 6);
        Assert.Throws<ArgumentNullException>(() => dice.Without(null!));
    }



    [Fact]
    public void WithModifier_returns_a_new_pool_with_the_modifier()
    {
        var dice = new Dice(2, 6, 1);

        var result = dice.WithModifier(5);

        Assert.Equal(5, result.Modifier);
        Assert.Equal(2, result.DieCount);
    }



    [Fact]
    public void WithModifier_does_not_modify_the_original()
    {
        var dice = new Dice(2, 6, 1);

        dice.WithModifier(5);

        Assert.Equal(1, dice.Modifier);
    }



    // ---- Immutability / read-only collection ----

    [Fact]
    public void Count_reflects_the_number_of_dice()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });
        Assert.Equal(2, dice.Count);
    }



    [Fact]
    public void GetHashCode_and_Equals_when_used_as_a_dictionary_key_find_an_equal_instance()
    {
        var key = new Dice(2, 6, 1);
        var map = new Dictionary<Dice, string> { [key] = "attack" };

        // An equal but distinct instance resolves to the same entry (stable hash + equality).
        var lookup = new Dice(2, 6, 1);
        Assert.True(map.ContainsKey(lookup));
        Assert.Equal("attack", map[lookup]);
    }



    [Fact]
    public void GetEnumerator_when_enumerated_yields_the_dice_in_order()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) });

        var sides = dice.Select(die => die.SideCount).ToList();

        Assert.Equal(new[] { 6, 4 }, sides);
    }



    [Fact]
    public void GetEnumerator_when_used_through_IEnumerable_yields_every_die()
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
    public void MinValue_when_pool_is_homogeneous_is_dieCount_plus_modifier(int dieCount, int sideCount, int modifier, int expectedValue)
    {
        var sut = new Dice(dieCount, sideCount, modifier);
        Assert.Equal(expectedValue, sut.MinValue);
    }



    [Theory]
    [InlineData(1, 6, 0, 6)]     // 1d6 = 6
    [InlineData(2, 8, 3, 19)]    // 2d8+3 = 19
    [InlineData(2, 10, -1, 19)]  // 2d10-1 = 19
    [InlineData(3, 4, -5, 7)]    // 3d4-5 = 7
    public void MaxValue_when_pool_is_homogeneous_is_dieCount_times_sideCount_plus_modifier(int dieCount, int sideCount, int modifier, int expectedValue)
    {
        var sut = new Dice(dieCount, sideCount, modifier);
        Assert.Equal(expectedValue, sut.MaxValue);
    }



    [Fact]
    public void MinValue_and_MaxValue_when_pool_is_heterogeneous_sum_each_die()
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
    public void Roll_when_default_pool_returns_value_within_bounds()
    {
        var dice = new Dice();
        var result = dice.Roll();
        Assert.InRange(result, dice.MinValue, dice.MaxValue);
    }



    [Fact]
    public void Roll_when_pool_is_heterogeneous_stays_within_bounds()
    {
        var dice = new Dice(new[] { new Die(6), new Die(4) }, 3);

        for (var i = 0; i < 1000; i++)
        {
            Assert.InRange(dice.Roll(), dice.MinValue, dice.MaxValue);
        }
    }



    [Fact]
    public void Roll_when_called_concurrently_stays_random_and_within_bounds()
    {
        // Arrange - a heterogeneous pool with a modifier: every die shares the one System.Random on the
        // legacy targets, so concurrent pool rolls contend on it from several dice per call.
        var dice = new Dice(new[] { new Die(6), new Die(6), new Die(4) }, 2);

        // Act
        var tail = DieTests.RollUnderContention(dice.Roll, dice.MinValue, dice.MaxValue, out var outOfRange);

        // Assert
        Assert.Equal(0, outOfRange);
        Assert.All(tail, roll => Assert.InRange(roll, dice.MinValue, dice.MaxValue));
        // A stuck System.Random makes every die return 1, so the whole pool returns MinValue forever.
        Assert.True(tail.Distinct().Count() > 1, "Roll() is stuck on one value after concurrent use.");
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
    public void Equals_when_parameters_match_returns_true()
    {
        Assert.Equal(new Dice(2, 6, 3), new Dice(2, 6, 3));
    }



    [Fact]
    public void Equals_when_dice_are_the_same_multiset_in_a_different_order_returns_true()
    {
        var dice1 = new Dice(new[] { new Die(6), new Die(4) }, 3);
        var dice2 = new Dice(new[] { new Die(4), new Die(6) }, 3);

        Assert.Equal(dice1, dice2);
        Assert.Equal(dice1.GetHashCode(), dice2.GetHashCode());
    }



    [Fact]
    public void Equals_when_DieCount_differs_returns_false()
    {
        Assert.NotEqual(new Dice(1, 6, 3), new Dice(2, 6, 3));
    }



    [Fact]
    public void Equals_when_SideCount_differs_returns_false()
    {
        Assert.NotEqual(new Dice(2, 6, 3), new Dice(2, 8, 3));
    }



    [Fact]
    public void Equals_when_Modifier_differs_returns_false()
    {
        Assert.NotEqual(new Dice(2, 6, 3), new Dice(2, 6, 4));
    }



    [Fact]
    public void Equals_when_other_Dice_is_null_returns_false()
    {
        var dice = new Dice(1, 6, 0);
        Dice? other = null;

#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(dice.Equals(other));
#pragma warning restore CA1508 // Avoid dead conditional code
    }



    [Fact]
    public void Equals_when_other_Dice_is_the_same_reference_returns_true()
    {
        var dice = new Dice(1, 6, 0);
        Assert.True(dice.Equals(dice));
    }



    [Fact]
    public void Equals_when_other_object_is_null_returns_false()
    {
        var dice = new Dice(1, 6, 0);
        object? other = null;
#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(dice.Equals(other));
#pragma warning restore CA1508 // Avoid dead conditional code
    }



    [Fact]
    public void Equals_when_other_object_is_the_same_reference_returns_true()
    {
        var dice = new Dice(1, 6, 0);
        Assert.True(dice.Equals((object)dice));
    }



    [Fact]
    public void Equals_when_other_object_is_a_different_type_returns_false()
    {
        var dice = new Dice(1, 6, 0);
        var notDice = new { DieCount = 1, Modifier = 0 };
        Assert.False(dice.Equals(notDice));
    }



    [Fact]
    public void Equals_when_other_object_is_an_equal_Dice_returns_true()
    {
        Assert.True(new Dice(2, 8, 1).Equals((object)new Dice(2, 8, 1)));
    }



    [Fact]
    public void GetHashCode_when_parameters_match_returns_the_same_value()
    {
        Assert.Equal(new Dice(2, 6, 3).GetHashCode(), new Dice(2, 6, 3).GetHashCode());
    }



    [Fact]
    public void GetHashCode_when_parameters_differ_returns_different_values()
    {
        Assert.NotEqual(new Dice(1, 8, 2).GetHashCode(), new Dice(2, 6, 3).GetHashCode());
    }



    [Fact]
    public void GetHashCode_combines_modifier_die_count_and_side_sum()
    {
        // ((modifier * 397) ^ dieCount) * 397 ^ sideSum, with modifier 3, dieCount 2, sideSum 6 + 6.
        // Pinning the exact value catches arithmetic changes to the combination that
        // a "same inputs, same hash" check cannot see.
        Assert.Equal(472045, new Dice(2, 6, 3).GetHashCode());
    }



    [Fact]
    public void Dice_implements_IDice()
    {
        IDice dice = new Dice();
        Assert.NotNull(dice);
        Assert.IsType<Dice>(dice);
    }



    [Fact]
    public void Dice_implements_IEquatable_Dice()
    {
        IEquatable<Dice> dice = new Dice();
        Assert.NotNull(dice);
        Assert.IsType<Dice>(dice);
    }



    [Fact]
    public void Dice_implements_IReadOnlyCollection_Die()
    {
        IReadOnlyCollection<Die> dice = new Dice();
        Assert.NotNull(dice);
        Assert.IsType<Dice>(dice);
    }



    // ---- TryParse ----

    [Theory]
    [InlineData("1d6", 1, 0, "1d6")]
    [InlineData("1d2", 1, 0, "1d2")]       // smallest valid side count
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
    [InlineData("2D6", 2, 0, "2d6")]           // the regex accepts [dD]; ToString always emits d
    [InlineData("2D6+1D4", 3, 0, "2d6+1d4")]
    [InlineData("+2d6", 2, 0, "2d6")]          // a leading + on a dice term is a no-op sign
    [InlineData("+3", 0, 3, "+3")]             // modifier-only notation: an empty pool with a modifier
    [InlineData("-2", 0, -2, "-2")]
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
    public void TryParse_when_dice_notation_is_invalid_fails_with_error_message(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Invalid dice notation format. Value must be in XdY+Z format.", result.ErrorMessage);
    }



    [Theory]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("")]
    public void TryParse_when_notation_is_null_or_whitespace_fails_with_error_message(string? notation)
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
    [InlineData("10000d6", 10_000)]       // exactly the cap
    [InlineData("9999d6+1d4", 10_000)]    // the cap reached across two terms
    public void TryParse_when_die_count_is_within_MaxDieCount_returns_new_Dice(string notation, int dieCount)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Succeeded);
        Assert.Equal(dieCount, result.Value!.DieCount);
    }



    [Theory]
    [InlineData("10001d6")]               // one over the cap in a single term
    [InlineData("9999d6+2d4")]            // one over the cap across two terms
    [InlineData("5000d6+5001d4")]         // each term valid alone, together over the cap
    [InlineData("2000000000d6")]          // the untrusted-input case: must fail, not allocate
    public void TryParse_when_die_count_exceeds_MaxDieCount_returns_failure(string notation)
    {
        var result = Dice.TryParse(notation);

        Assert.True(result.Failed);
        Assert.Equal("Die count must be at most 10000.", result.ErrorMessage);
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
