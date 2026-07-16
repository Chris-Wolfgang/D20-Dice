namespace Wolfgang.D20;

/// <summary>
/// Extension methods that compute the average roll of a die or collection of dice, rounded to a whole number.
/// </summary>
/// <remarks>
/// The average roll is the midpoint of the possible results, <c>(MinValue + MaxValue) / 2</c>, which is always
/// a whole number or a half. These methods round that midpoint to an <see cref="int"/>, which is convenient for
/// tabletop rules that call for average damage rounded a particular way &#8212; for example, half damage on a
/// successful save is conventionally rounded down.
/// </remarks>
public static class AverageRollExtensions
{

    /// <summary>
    /// Returns the average roll of the die, rounded up to the nearest whole number.
    /// </summary>
    /// <param name="die">The die to average.</param>
    /// <returns>
    /// The midpoint of <see cref="IDie.MinValue"/> and <see cref="IDie.MaxValue"/>, rounded up.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="die"/> is null.</exception>
    /// <example>
    /// <code>
    /// int average = new Die(6).AverageRoundedUp(); // 3.5 -> 4
    /// </code>
    /// </example>
    public static int AverageRoundedUp(this IDie die)
    {
        if (die is null)
        {
            throw new ArgumentNullException(nameof(die));
        }

        return (int)Math.Ceiling(Average(die.MinValue, die.MaxValue));
    }



    /// <summary>
    /// Returns the average roll of the dice, including the modifier, rounded up to the nearest whole number.
    /// </summary>
    /// <param name="dice">The dice to average.</param>
    /// <returns>
    /// The midpoint of <see cref="IDice.MinValue"/> and <see cref="IDice.MaxValue"/>, rounded up.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="dice"/> is null.</exception>
    /// <example>
    /// <code>
    /// int average = new Dice(2, 6).AverageRoundedUp(); // 7.0 -> 7
    /// </code>
    /// </example>
    public static int AverageRoundedUp(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return (int)Math.Ceiling(Average(dice.MinValue, dice.MaxValue));
    }



    /// <summary>
    /// Returns the average roll of the die, rounded down to the nearest whole number.
    /// </summary>
    /// <param name="die">The die to average.</param>
    /// <returns>
    /// The midpoint of <see cref="IDie.MinValue"/> and <see cref="IDie.MaxValue"/>, rounded down.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="die"/> is null.</exception>
    /// <example>
    /// <code>
    /// int average = new Die(6).AverageRoundedDown(); // 3.5 -> 3
    /// </code>
    /// </example>
    public static int AverageRoundedDown(this IDie die)
    {
        if (die is null)
        {
            throw new ArgumentNullException(nameof(die));
        }

        return (int)Math.Floor(Average(die.MinValue, die.MaxValue));
    }



    /// <summary>
    /// Returns the average roll of the dice, including the modifier, rounded down to the nearest whole number.
    /// </summary>
    /// <param name="dice">The dice to average.</param>
    /// <returns>
    /// The midpoint of <see cref="IDice.MinValue"/> and <see cref="IDice.MaxValue"/>, rounded down.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="dice"/> is null.</exception>
    /// <example>
    /// <code>
    /// // 2d6 averages 7; half damage on a successful save, rounded down, is 3.
    /// int half = new Dice(2, 6).AverageRoundedDown() / 2; // 3
    /// </code>
    /// </example>
    public static int AverageRoundedDown(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return (int)Math.Floor(Average(dice.MinValue, dice.MaxValue));
    }



    /// <summary>
    /// Computes the midpoint of the inclusive range [<paramref name="minValue"/>, <paramref name="maxValue"/>].
    /// </summary>
    /// <param name="minValue">The minimum value that can be rolled.</param>
    /// <param name="maxValue">The maximum value that can be rolled.</param>
    /// <returns>The average of the two bounds, always a whole number or a half.</returns>
    private static decimal Average(int minValue, int maxValue)
    {
        // Promote to decimal before adding so the sum cannot overflow int and the .0 / .5 result stays exact.
        return ((decimal)minValue + maxValue) / 2m;
    }

}
