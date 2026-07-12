#if NET8_0_OR_GREATER
using System.Collections.Generic;
using CsCheck;
using Xunit;

namespace Wolfgang.D20.Tests.Unit;

/// <summary>
/// Property-based tests (CsCheck) that assert invariants hold across a wide range of randomly generated
/// dice. Guarded to net8.0+, the target framework CsCheck 4.x supports; the framework-independent behaviour
/// is exercised on every other TFM by the example-based tests.
/// </summary>
public class PropertyTests
{

    // A single die with a side count anywhere in the valid range.
    private static readonly Gen<Die> GenDie = Gen.Int[2, 1000].Select(sideCount => new Die(sideCount));



    // A pool of 1-8 dice with a modifier in a range that cannot overflow when summed.
    private static readonly Gen<Dice> GenDice =
        Gen.Select(GenDie.List[1, 8], Gen.Int[-100, 100], (dice, modifier) => new Dice(dice, modifier));



    [Fact]
    public void Die_Roll_is_always_within_its_bounds()
    {
        GenDie.Sample(die =>
        {
            var roll = die.Roll();
            return roll >= die.MinValue && roll <= die.MaxValue;
        });
    }



    [Fact]
    public void Dice_Roll_is_always_within_its_bounds()
    {
        GenDice.Sample(dice =>
        {
            var roll = dice.Roll();
            return roll >= dice.MinValue && roll <= dice.MaxValue;
        });
    }



    [Fact]
    public void Dice_MinValue_never_exceeds_MaxValue()
    {
        GenDice.Sample(dice => dice.MinValue <= dice.MaxValue);
    }



    [Fact]
    public void Dice_ToString_round_trips_through_TryParse()
    {
        GenDice.Sample(dice =>
        {
            var parsed = Dice.TryParse(dice.ToString());
            return parsed.Succeeded && dice.Equals(parsed.Value);
        });
    }

}
#endif
