namespace Wolfgang.D20;

/// <summary>
/// Represents a collection of <see cref="Die"/> rolled together, with an optional flat modifier.
/// </summary>
/// <remarks>
/// The dice need not be homogeneous; a single <see cref="Dice"/> can contain dice with differing
/// side counts, for example <c>2d6+1d4+3</c>.
/// </remarks>
public interface IDice
{

    /// <summary>
    /// The number of dice in the collection.
    /// </summary>
    int DieCount { get; }


    /// <summary>
    /// An optional modifier to add to the total of the dice rolled.
    /// </summary>
    /// <remarks>
    /// The value can be positive or negative, and can be used to adjust the result of the roll.
    /// </remarks>
    int Modifier { get; }


    /// <summary>
    /// The minimum value that can be rolled with the dice in the collection and the modifier.
    /// </summary>
    int MinValue { get; }


    /// <summary>
    /// The maximum value that can be rolled with the dice in the collection and the modifier.
    /// </summary>
    int MaxValue { get; }


    /// <summary>
    /// Rolls every die in the collection and returns the total value rolled, including any modifier.
    /// </summary>
    /// <returns>
    /// The sum of an independent roll of each die in the collection plus <see cref="Modifier"/>.
    /// Always between <see cref="MinValue"/> and <see cref="MaxValue"/> inclusive.
    /// </returns>
    int Roll();


    /// <summary>
    /// Returns a string representation of the dice in standard dice notation, for example <c>2d6+1d4+3</c>.
    /// </summary>
    /// <returns>
    /// The dice grouped by side count in descending order, followed by the modifier; the modifier is
    /// omitted when zero, and a negative modifier renders with a leading minus sign.
    /// </returns>
    string ToString();

}
