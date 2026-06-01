namespace Wolfgang.D20;

/// <summary>
/// Represents a number of dice, each with the specified number of sides and an optional modifier.
/// </summary>
public interface IDice
{

    /// <summary>
    /// The number of dice to roll. Must be greater than 0.
    /// </summary>
    int DieCount { get; }


    /// <summary>
    /// The number of sides on each die. Must be at least 2.
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
    /// <returns>
    /// The sum of <see cref="DieCount"/> independent uniform rolls in [1, <see cref="SideCount"/>] plus <see cref="Modifier"/>.
    /// Always between <see cref="MinValue"/> and <see cref="MaxValue"/> inclusive.
    /// </returns>
    int Roll();



    /// <summary>
    /// Returns a string representation of the dice in the format "XdY+Z" where:
    /// X is the number of dice,
    /// Y is the number of sides on each die,
    /// Z is the modifier (if any).
    /// </summary>
    /// <returns>
    /// The dice in standard <c>XdY+Z</c> notation; the modifier is omitted when zero, and a negative modifier
    /// renders as <c>XdY-Z</c>.
    /// </returns>
    string ToString();

}
