namespace Wolfgang.D20;

/// <summary>
/// Represents a number of dice, each with the name specified number of sides and optional modifier.
/// </summary>
public interface IDice
{

    /// <summary>
    /// The number of dice to roll. Must be greater than 0.
    /// </summary>
    int DieCount { get; }


    /// <summary>
    /// The number of sides on each die. Must be greater than 2.
    /// </summary>
    /// <remarks>
    /// A value of 2 represents a coin toss, 3 represents a three-sided die, etc.
    /// </remarks>
    int SideCount { get; }


    /// <summary>
    /// An optional modifier to add to the total of the dice rolled.
    /// </summary>
    /// <remarks>
    /// The value can be positive or negative, and can be used to adjust the result of the roll.
    /// </remarks>
    int Modifier { get; }


    /// <summary>
    /// The minimum value that can be rolled with the specified dice and modifier.
    /// </summary>
    int MinValue { get; }


    /// <summary>
    /// The maximum value that can be rolled with the specified dice and modifier.
    /// </summary>
    int MaxValue { get; }


    /// <summary>
    /// Rolls the dice and returns the total value rolled, including any modifier.
    /// </summary>
    /// <returns>int</returns>
    int Roll();



    /// <summary>
    /// Returns a string representation of the dice in the format "XdY+Z" where:
    /// x is the number of dice,
    /// y is the number of sides on each die,
    /// z is the modifier (if any).
    /// </summary>
    /// <returns>string</returns>
    /// <remarks>
    /// If the modifier is 0, it is omitted from the string.
    /// </remarks>
    string ToString();

}
