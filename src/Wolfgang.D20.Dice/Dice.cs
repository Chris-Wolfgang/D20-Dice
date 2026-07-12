using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Wolfgang.TryPattern;

namespace Wolfgang.D20;

/// <summary>
/// Represents a collection of <see cref="Die"/> rolled together, with an optional flat modifier.
/// </summary>
/// <remarks>
/// The dice need not be homogeneous; a single <see cref="Dice"/> can contain dice with differing
/// side counts, for example <c>2d6+1d4+3</c>. Individual dice can be added and removed after construction.
/// </remarks>
public sealed class Dice : IDice, ICollection<Die>, IEquatable<Dice>
{
    private readonly List<Die> _dice = new();



    /// <summary>
    /// Constructs a new instance of <see cref="Dice"/> containing the specified number of identical dice
    /// and an optional modifier.
    /// </summary>
    /// <param name="dieCount">The number of dice</param>
    /// <param name="sideCount">The number of sides on each die</param>
    /// <param name="modifier">An optional modifier to add to the result</param>
    /// <exception cref="ArgumentOutOfRangeException">dieCount is less than 1</exception>
    /// <exception cref="ArgumentOutOfRangeException">sideCount is less than 2</exception>
    /// <remarks>
    /// This convenience constructor builds a homogeneous collection of <paramref name="dieCount"/> dice,
    /// each with <paramref name="sideCount"/> sides. A sideCount of 2 represents a coin toss.
    /// </remarks>
    public Dice(int dieCount = 1, int sideCount = 6, int modifier = 0)
    {
        if (dieCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(dieCount));
        }

        // Die validates sideCount; surface the same ArgumentOutOfRangeException parameter name.
        if (sideCount < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(sideCount));
        }

        for (var i = 0; i < dieCount; i++)
        {
            _dice.Add(new Die(sideCount));
        }
        Modifier = modifier;
    }



    /// <summary>
    /// Constructs a new instance of <see cref="Dice"/> from an existing sequence of <see cref="Die"/>
    /// and an optional modifier.
    /// </summary>
    /// <param name="dice">The dice to include in the collection</param>
    /// <param name="modifier">An optional modifier to add to the result</param>
    /// <exception cref="ArgumentNullException"><paramref name="dice"/> is null</exception>
    /// <exception cref="ArgumentException"><paramref name="dice"/> contains a null element</exception>
    public Dice(IEnumerable<Die> dice, int modifier = 0)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        foreach (var die in dice)
        {
            if (die is null)
            {
                throw new ArgumentException("Sequence cannot contain a null die.", nameof(dice));
            }
            _dice.Add(die);
        }
        Modifier = modifier;
    }



    /// <summary>
    /// The number of dice in the collection.
    /// </summary>
    public int DieCount => _dice.Count;



    /// <summary>
    /// The number of dice in the collection. <see cref="ICollection{T}"/> implementation; equal to <see cref="DieCount"/>.
    /// </summary>
    public int Count => _dice.Count;



    /// <summary>
    /// Always false; the collection is mutable. <see cref="ICollection{T}"/> implementation.
    /// </summary>
    public bool IsReadOnly => false;



    /// <summary>
    /// An optional modifier to add to the total of the dice rolled.
    /// </summary>
    /// <remarks>
    /// The value can be positive or negative, and can be used to adjust the result of the roll.
    /// </remarks>
    public int Modifier { get; set; }



    /// <summary>
    /// The minimum value that can be rolled with the dice in the collection and the modifier.
    /// </summary>
    public int MinValue
    {
        get
        {
            checked
            {
                // Each die has a minimum of 1, so the sum of the minimums equals the die count.
                return DieCount + Modifier;
            }
        }
    }



    /// <summary>
    /// The maximum value that can be rolled with the dice in the collection and the modifier.
    /// </summary>
    public int MaxValue
    {
        get
        {
            checked
            {
                // foreach over the concrete List<Die> uses its struct enumerator and allocates nothing,
                // unlike Enumerable.Sum which boxes the enumerator. The checked context guards overflow.
                var sum = Modifier;
                foreach (var die in _dice)
                {
                    sum += die.SideCount;
                }
                return sum;
            }
        }
    }



    /// <summary>
    /// Rolls every die in the collection and returns the total value rolled, including any modifier.
    /// </summary>
    /// <returns>
    /// The sum of an independent roll of each die in the collection plus <see cref="Modifier"/>.
    /// Always between <see cref="MinValue"/> and <see cref="MaxValue"/> inclusive.
    /// </returns>
    public int Roll()
    {
        checked
        {
            // Iterate the List<Die> directly rather than via Enumerable.Sum: a foreach over the concrete
            // list uses its struct enumerator, so this hot path allocates nothing (LINQ would box the
            // enumerator on the heap). The checked context still guards the running total against overflow.
            var total = Modifier;
            foreach (var die in _dice)
            {
                total += die.Roll();
            }
            return total;
        }
    }



    /// <summary>
    /// Adds a die to the collection.
    /// </summary>
    /// <param name="item">The die to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="item"/> is null</exception>
    public void Add(Die item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        _dice.Add(item);
    }



    /// <summary>
    /// Removes the first occurrence of a die with the same number of sides from the collection.
    /// </summary>
    /// <param name="item">The die to remove.</param>
    /// <returns>true if a matching die was removed; otherwise, false.</returns>
    public bool Remove(Die item)
    {
        if (item is null)
        {
            return false;
        }
        return _dice.Remove(item);
    }



    /// <summary>
    /// Removes the die at the specified index in the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the die to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0 or not less than <see cref="DieCount"/>.
    /// </exception>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _dice.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        _dice.RemoveAt(index);
    }



    /// <summary>
    /// Removes all dice from the collection. The <see cref="Modifier"/> is left unchanged.
    /// </summary>
    public void Clear()
    {
        _dice.Clear();
    }



    /// <summary>
    /// Determines whether the collection contains a die with the same number of sides.
    /// </summary>
    /// <param name="item">The die to locate.</param>
    /// <returns>true if a matching die is found; otherwise, false.</returns>
    public bool Contains(Die item)
    {
        if (item is null)
        {
            return false;
        }
        return _dice.Contains(item);
    }



    /// <summary>
    /// Copies the dice in the collection to an array, starting at the specified index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    public void CopyTo(Die[] array, int arrayIndex)
    {
        _dice.CopyTo(array, arrayIndex);
    }



    /// <summary>
    /// Returns an enumerator that iterates through the dice in the collection.
    /// </summary>
    /// <returns>An enumerator over the dice.</returns>
    public IEnumerator<Die> GetEnumerator()
    {
        return _dice.GetEnumerator();
    }



    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }



    /// <summary>
    /// Returns a string representation of the dice in standard dice notation, for example <c>2d6+1d4+3</c>.
    /// </summary>
    /// <returns>
    /// The dice grouped by side count (in the order each side count first appears), followed by the
    /// modifier. Dice order is not significant. The modifier is omitted when zero, and a negative
    /// modifier renders with a leading minus sign. An empty collection renders only the modifier
    /// (or an empty string when the modifier is zero).
    /// </returns>
    public override string ToString()
    {
        var builder = new StringBuilder();

        // Dice order is not significant (1d4+1d8 == 1d8+1d4); group by side count in the order each
        // side count first appears. The flat modifier, if any, always comes last.
        var groups = _dice.GroupBy(die => die.SideCount);

        var first = true;
        foreach (var group in groups)
        {
            if (!first)
            {
                builder.Append('+');
            }
            builder.Append(group.Count()).Append('d').Append(group.Key);
            first = false;
        }

        switch (Modifier)
        {
            case > 0:
                builder.Append('+').Append(Modifier);
                break;
            case < 0:
                builder.Append(Modifier);
                break;
        }

        return builder.ToString();
    }



    /// <summary>
    /// Checks if this instance is equal to another instance of <see cref="Dice"/>.
    /// </summary>
    /// <param name="other">
    /// The other instance of <see cref="Dice"/> to compare with this instance.
    /// </param>
    /// <returns>
    /// True if both instances have the same <see cref="Modifier"/> and the same multiset of dice
    /// (matching side counts, regardless of order); otherwise, false.
    /// </returns>
    public bool Equals(Dice? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (Modifier != other.Modifier || DieCount != other.DieCount)
        {
            return false;
        }

        var theseSides = _dice.Select(die => die.SideCount).OrderBy(sides => sides);
        var otherSides = other._dice.Select(die => die.SideCount).OrderBy(sides => sides);
        return theseSides.SequenceEqual(otherSides);
    }



    /// <summary>
    /// Checks if this instance is equal to another object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance. It can be null or of any type.
    /// </param>
    /// <returns>
    /// True if the object is a <see cref="Dice"/> instance equal to this instance; otherwise, false.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Dice)obj);
    }



    /// <summary>
    /// Generates a hash code for this instance based on its <see cref="Modifier"/> and the multiset of
    /// dice side counts (order-independent).
    /// </summary>
    /// <returns>A combined hash code.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            // Order-independent: sum the per-die hashes so equal multisets hash identically.
            var hashCode = Modifier;
            var sideSum = 0;
            foreach (var die in _dice)
            {
                sideSum += die.SideCount;
            }
            hashCode = (hashCode * 397) ^ DieCount;
            hashCode = (hashCode * 397) ^ sideSum;
            return hashCode;
        }
    }



    private static readonly Regex TokenRegex = new
    (
        @"\G(?<sign>[+-]?)(?:(?<dieCount>\d*)[dD](?<sideCount>\d+)|(?<modifier>\d+))",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(1)
    );



    /// <summary>
    /// Tries to parse a string representation of dice notation into a <see cref="Dice"/> instance.
    /// </summary>
    /// <param name="notation">
    /// The dice notation to parse, for example <c>2d6+1d4+3</c>. Whitespace is ignored. The notation may
    /// contain any number of dice terms (<c>XdY</c>) and flat modifiers (<c>+Z</c> / <c>-Z</c>).
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the parsed <see cref="Dice"/> instance if successful;
    /// otherwise, a failed result with <see cref="Wolfgang.TryPattern.Result.ErrorMessage"/> describing the failure.
    /// Accessing <see cref="Wolfgang.TryPattern.Result{T}.Value"/> on a failed result throws <see cref="InvalidOperationException"/>.
    /// </returns>
    public static Result<Dice?> TryParse(string? notation)
    {
        if (string.IsNullOrWhiteSpace(notation))
        {
            return Result<Dice?>.Failure("Value cannot be null or empty.");
        }

        // Remove all whitespace so "2d6 + 1d4 + 3" parses the same as "2d6+1d4+3".
        var compact = new StringBuilder(notation!.Length);
        foreach (var character in notation)
        {
            if (!char.IsWhiteSpace(character))
            {
                compact.Append(character);
            }
        }
        // IsNullOrWhiteSpace above guarantees at least one non-whitespace character remains.
        var text = compact.ToString();

        var dice = new List<Die>();
        var modifier = 0;
        var position = 0;

        while (position < text.Length)
        {
            var match = TokenRegex.Match(text, position);
            if (!match.Success || match.Index != position)
            {
                return Result<Dice?>.Failure("Invalid dice notation format. Value must be in XdY+Z format.");
            }

            // Each token contributes either dice (added to the list) or a modifier delta.
            var tokenResult = TryParseToken(match, dice);
            if (tokenResult.Failed)
            {
                return Result<Dice?>.Failure(tokenResult.ErrorMessage!);
            }

            try
            {
                checked
                {
                    modifier += tokenResult.Value;
                }
            }
            catch (OverflowException)
            {
                return Result<Dice?>.Failure("Modifier value is out of range.");
            }

            position = match.Index + match.Length;
        }

        return Result<Dice?>.Success(new Dice(dice, modifier));
    }



    /// <summary>
    /// Parses a single matched token, appending any dice to <paramref name="dice"/> and returning the
    /// modifier delta the token contributes (zero for a dice term).
    /// </summary>
    private static Result<int> TryParseToken(Match match, List<Die> dice)
    {
        var negative = match.Groups["sign"].Value.Equals("-", StringComparison.Ordinal);

        if (!match.Groups["sideCount"].Success)
        {
            return TryGetModifierValue(match.Groups["modifier"].Value, negative);
        }

        if (negative)
        {
            // A negative dice term (e.g. "-2d6") is not supported.
            return Result<int>.Failure("Invalid dice notation format. Value must be in XdY+Z format.");
        }

        var dieCountResult = TryGetDieCount(match.Groups["dieCount"].Value);
        if (dieCountResult.Failed)
        {
            return Result<int>.Failure(dieCountResult.ErrorMessage!);
        }

        var sideCountResult = TryGetSideCount(match.Groups["sideCount"].Value);
        if (sideCountResult.Failed)
        {
            return Result<int>.Failure(sideCountResult.ErrorMessage!);
        }

        var die = new Die(sideCountResult.Value);
        for (var i = 0; i < dieCountResult.Value; i++)
        {
            dice.Add(die);
        }

        return Result<int>.Success(0);
    }



    private static Result<int> TryGetDieCount(string value)
    {
        // If not specified assume 1 i.e. "d6" is the same as "1d6".
        if (string.IsNullOrEmpty(value))
        {
            return Result<int>.Success(1);
        }

        if (!int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var dieCount))
        {
            return Result<int>.Failure("Die count value is out of range.");
        }

        if (dieCount < 1)
        {
            return Result<int>.Failure("Die count must be greater than 0.");
        }

        return Result<int>.Success(dieCount);
    }



    private static Result<int> TryGetSideCount(string value)
    {
        if (!int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var sideCount))
        {
            return Result<int>.Failure("Side count value is out of range.");
        }

        if (sideCount < 2)
        {
            return Result<int>.Failure("Side count must be greater than 1.");
        }

        return Result<int>.Success(sideCount);
    }



    private static Result<int> TryGetModifierValue(string value, bool negative)
    {
        if (!int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var modifierValue))
        {
            return Result<int>.Failure("Modifier value is out of range.");
        }

        // int.Parse of the magnitude cannot represent the negation of int.MinValue, but the magnitude
        // itself never exceeds int.MaxValue here, so negation is always safe.
        return Result<int>.Success(negative ? -modifierValue : modifierValue);
    }
}
