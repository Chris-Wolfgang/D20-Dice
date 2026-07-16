namespace Wolfgang.D20;

/// <summary>
/// Represents a single die with a fixed number of sides.
/// </summary>
/// <example>
/// <code>
/// IDie die = new Die(20);
/// int result = die.Roll(); // a value in [1, 20]
/// </code>
/// </example>
public interface IDie
{

    /// <summary>
    /// The number of sides on the die. Must be at least 2.
    /// </summary>
    /// <remarks>
    /// A value of 2 represents a coin toss, 3 represents a three-sided die, etc.
    /// </remarks>
    int SideCount { get; }


    /// <summary>
    /// The minimum value that can be rolled on the die. Always 1.
    /// </summary>
    int MinValue { get; }


    /// <summary>
    /// The maximum value that can be rolled on the die. Equal to <see cref="SideCount"/>.
    /// </summary>
    int MaxValue { get; }


    /// <summary>
    /// Rolls the die and returns the value rolled.
    /// </summary>
    /// <returns>
    /// A uniform random value in [<see cref="MinValue"/>, <see cref="MaxValue"/>] inclusive.
    /// </returns>
    int Roll();


    /// <summary>
    /// Returns a string representation of the die in the format "dY" where Y is the number of sides.
    /// </summary>
    /// <returns>The die in standard <c>dY</c> notation.</returns>
    string ToString();

}
