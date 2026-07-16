using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Wolfgang.TryPattern;

namespace Wolfgang.D20;

/// <summary>
/// Represents a collection of <see cref="Die"/> rolled together, with an optional flat modifier.
/// </summary>
/// <remarks>
/// The dice need not be homogeneous; a single <see cref="Dice"/> can contain dice with differing
/// side counts, for example <c>2d6+1d4+3</c>. <see cref="Dice"/> is immutable: the "with" builders
/// (<see cref="WithDie"/>, <see cref="Without"/>, <see cref="WithModifier"/>) return a new instance
/// rather than modifying the current one, so an instance is safe to use as a dictionary key and to
/// share across threads.
/// </remarks>
public sealed class Dice : IDice, IReadOnlyCollection<Die>, IEquatable<Dice>
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
    /// <example>
    /// <code>
    /// // 2d6+3: two six-sided dice plus a flat +3.
    /// var attack = new Dice(dieCount: 2, sideCount: 6, modifier: 3);
    /// int total = attack.Roll(); // a value in [5, 15]
    /// </code>
    /// </example>
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
    /// <example>
    /// <code>
    /// // 1d6+1d4+1: a heterogeneous pool built from individual dice.
    /// var dice = new Dice(new[] { new Die(6), new Die(4) }, modifier: 1);
    /// </code>
    /// </example>
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
    /// The number of dice in the collection. <see cref="IReadOnlyCollection{T}"/> implementation; equal to <see cref="DieCount"/>.
    /// </summary>
    public int Count => _dice.Count;



    /// <summary>
    /// An optional modifier to add to the total of the dice rolled.
    /// </summary>
    /// <remarks>
    /// The value can be positive or negative, and can be used to adjust the result of the roll.
    /// It is fixed at construction; use <see cref="WithModifier"/> to obtain a copy with a different modifier.
    /// </remarks>
    /// <example>
    /// <code>
    /// var attack = new Dice(1, 20, 2);        // 1d20+2 — modifier set at construction
    /// var blessed = attack.WithModifier(5);   // 1d20+5 — a new pool; 'attack' is unchanged
    /// </code>
    /// </example>
    public int Modifier { get; }



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
    /// <example>
    /// <code>
    /// var dice = new Dice(3, 6); // 3d6
    /// int total = dice.Roll(); // a value in [3, 18]
    /// </code>
    /// </example>
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
    /// Returns a new <see cref="Dice"/> containing every die in this instance plus <paramref name="die"/>,
    /// keeping the same <see cref="Modifier"/>. This instance is not modified.
    /// </summary>
    /// <param name="die">The die to add.</param>
    /// <returns>A new <see cref="Dice"/> with <paramref name="die"/> appended.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="die"/> is null.</exception>
    /// <example>
    /// <code>
    /// var pool = new Dice(2, 6).WithDie(new Die(4)); // 2d6 -> 2d6+1d4
    /// </code>
    /// </example>
    public Dice WithDie(Die die)
    {
        if (die is null)
        {
            throw new ArgumentNullException(nameof(die));
        }

        // The Dice(IEnumerable<Die>, int) constructor copies the sequence, so the new instance
        // never shares this instance's backing list.
        var dice = new List<Die>(_dice) { die };
        return new Dice(dice, Modifier);
    }



    /// <summary>
    /// Returns a new <see cref="Dice"/> with the first die matching <paramref name="die"/> removed,
    /// keeping the same <see cref="Modifier"/>. If no die matches, an equal copy is returned. This
    /// instance is not modified.
    /// </summary>
    /// <param name="die">The die to remove.</param>
    /// <returns>A new <see cref="Dice"/> without the first matching die.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="die"/> is null.</exception>
    public Dice Without(Die die)
    {
        if (die is null)
        {
            throw new ArgumentNullException(nameof(die));
        }

        var dice = new List<Die>(_dice);
        dice.Remove(die);
        return new Dice(dice, Modifier);
    }



    /// <summary>
    /// Returns a new <see cref="Dice"/> with the same dice but the specified <paramref name="modifier"/>.
    /// This instance is not modified.
    /// </summary>
    /// <param name="modifier">The flat modifier for the new instance.</param>
    /// <returns>A new <see cref="Dice"/> whose <see cref="Modifier"/> is <paramref name="modifier"/>.</returns>
    /// <example>
    /// <code>
    /// var attack = new Dice(1, 20, 2);      // 1d20+2
    /// var buffed = attack.WithModifier(5);  // 1d20+5 — derived from attack, which is unchanged
    /// </code>
    /// </example>
    public Dice WithModifier(int modifier)
    {
        return new Dice(_dice, modifier);
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
    /// <example>
    /// <code>
    /// string notation = new Dice(2, 6, 3).ToString(); // "2d6+3"
    /// </code>
    /// </example>
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
            // Format counts with the invariant culture so the notation always uses ASCII digits and
            // round-trips through TryParse regardless of the current thread culture.
            builder
                .Append(group.Count().ToString(CultureInfo.InvariantCulture))
                .Append('d')
                .Append(group.Key.ToString(CultureInfo.InvariantCulture));
            first = false;
        }

        switch (Modifier)
        {
            case > 0:
                // The invariant '+' is a literal; only the magnitude needs invariant formatting.
                builder.Append('+').Append(Modifier.ToString(CultureInfo.InvariantCulture));
                break;
            case < 0:
                // Invariant formatting emits the ASCII '-' sign, matching what TryParse accepts.
                builder.Append(Modifier.ToString(CultureInfo.InvariantCulture));
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
    /// <remarks>
    /// <see cref="Dice"/> is immutable, so this hash code is stable for the life of the instance and a
    /// <see cref="Dice"/> is safe to use as a key in a hashed collection such as
    /// <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/> or
    /// <see cref="System.Collections.Generic.HashSet{T}"/>.
    /// </remarks>
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
    /// <example>
    /// <code>
    /// var result = Dice.TryParse("2d6+3");
    /// if (result.Succeeded)
    /// {
    ///     Dice dice = result.Value!;
    ///     int total = dice.Roll(); // a value in [5, 15]
    /// }
    /// </code>
    /// </example>
    public static Result<Dice?> TryParse(string? notation)
    {
        if (string.IsNullOrWhiteSpace(notation))
        {
            return Result<Dice?>.Failure("Value cannot be null or empty.");
        }

        // Remove all whitespace so "2d6 + 1d4 + 3" parses the same as "2d6+1d4+3".
        // Kept as an explicit StringBuilder loop rather than a LINQ expression
        // (e.g. new string(notation.Where(...).ToArray())): the LINQ form allocates an
        // intermediate IEnumerator plus a char[] per call, whereas this appends straight
        // into a right-sized StringBuilder. notation! is required on target frameworks
        // whose string.IsNullOrWhiteSpace lacks [NotNullWhen] (net462/netstandard2.0).
        // ReSharper disable once LoopCanBeConvertedToLinq
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

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var dieCount))
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
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var sideCount))
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
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var modifierValue))
        {
            return Result<int>.Failure("Modifier value is out of range.");
        }

        // int.Parse of the magnitude cannot represent the negation of int.MinValue, but the magnitude
        // itself never exceeds int.MaxValue here, so negation is always safe.
        return Result<int>.Success(negative ? -modifierValue : modifierValue);
    }
}
