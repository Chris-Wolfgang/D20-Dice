using System.Text.RegularExpressions;
using Wolfgang.TryPattern;

namespace Wolfgang.D20;

/// <summary>
/// Represents a number of dice, each with the specified number of sides and an optional modifier.
/// </summary>
public class Dice : IDice, IEquatable<Dice>, IEqualityComparer<Dice>
{

    /// <summary>
    /// Constructs a new instance of <see cref="Dice"/> with the specified number of dice, sides, and modifier.
    /// </summary>
    /// <param name="dieCount">The number of dice</param>
    /// <param name="sideCount">The number of sides on each die</param>
    /// <param name="modifier">An optional modifier to add to the result</param>
    /// <exception cref="ArgumentOutOfRangeException">dieCount is less than 1</exception>
    /// <exception cref="ArgumentOutOfRangeException">sideCount is less than 2</exception>
    /// <remarks>sideCount of 2 represents a coin toss</remarks>
    public Dice(int dieCount = 1, int sideCount = 6, int modifier = 0)
    {
        if (dieCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(dieCount));
        }

        if (sideCount < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(sideCount));
        }
        DieCount = dieCount;
        SideCount = sideCount;
        Modifier = modifier;
    }



    /// <summary>
    /// The number of dice to roll. Must be greater than 0.
    /// </summary>
    public int DieCount { get; }



    /// <summary>
    /// The number of sides on each die. Must be greater than 2.
    /// </summary>
    /// <remarks>
    /// A value of 2 represents a coin toss, 3 represents a three-sided die, etc.
    /// </remarks>
    public int SideCount { get; }



    /// <summary>
    /// An optional modifier to add to the total of the dice rolled.
    /// </summary>
    /// <remarks>
    /// The value can be positive or negative, and can be used to adjust the result of the roll.
    /// </remarks>
    public int Modifier { get; }



    /// <summary>
    /// The minimum value that can be rolled with the specified dice and modifier.
    /// </summary>
    public int MinValue => (DieCount * 1) + Modifier;



    /// <summary>
    /// The maximum value that can be rolled with the specified dice and modifier.
    /// </summary>
    public int MaxValue => (DieCount * SideCount) + Modifier;



    /// <summary>
    /// Rolls the dice and returns the total value rolled, including any modifier.
    /// </summary>
    /// <returns>int</returns>
    public int Roll()
    {
        var random = new Random();
        var total = 0;
        for (var i = 0; i < DieCount; i++)
        {
            total += random.Next(1, SideCount);
        }
        return total + Modifier;
    }



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
    public override string ToString()
    {
        var value = $"{DieCount}d{SideCount}";
        return Modifier switch
        {
            0 => value,
            > 0 => $"{value}+{Modifier}",
            < 0 => $"{value}{Modifier}",
        };
    }



    /// <summary>
    /// Checks if this instance is equal to another instance of <see cref="Dice"/>.
    /// </summary>
    /// <param name="other">
    /// The other instance of <see cref="Dice"/> to compare with this instance.
    /// </param>
    /// <returns>
    /// True if both instances have the same DieCount, SideCount, and Modifier; otherwise, false.
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
        return DieCount == other.DieCount
               && SideCount == other.SideCount
               && Modifier == other.Modifier;
    }



    /// <summary>
    /// Checks if this instance is equal to another object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance. It can be null or of any type.
    /// </param>
    /// <returns>
    /// True if the object is a <see cref="Dice"/> instance and has the same DieCount, SideCount, and Modifier as this instance; otherwise, false.
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
    /// Determines whether two <see cref="Dice"/> instances are equal.
    /// </summary>
    /// <param name="x">The first <see cref="Dice"/> instance.</param>
    /// <param name="y">The second <see cref="Dice"/> instance.</param>
    /// <returns>true if the two instances are equal; otherwise, false.</returns>
    public bool Equals(Dice? x, Dice? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.DieCount == y.DieCount && x.SideCount == y.SideCount && x.Modifier == y.Modifier;
    }



    /// <summary>
    /// Generates a hash code for this instance based on its DieCount, SideCount, and Modifier.
    /// </summary>
    /// <param name="obj">The <see cref="Dice"/> instance for which to generate the hash code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="obj"/> is null.</exception>
    /// <returns>int</returns>
    public int GetHashCode(Dice obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        unchecked
        {
            var hashCode = obj.DieCount;
            hashCode = (hashCode * 397) ^ obj.SideCount;
            hashCode = (hashCode * 397) ^ obj.Modifier;
            return hashCode;
        }
    }



    /// <summary>
    /// Generates a hash code for this instance based on its DieCount, SideCount, and Modifier.
    /// </summary>
    /// <returns>int</returns>
    public override int GetHashCode()
    {
#if NET5_0_OR_GREATER
        return HashCode.Combine(DieCount, SideCount, Modifier);
#else
        unchecked
        {
            var hashCode = DieCount;
            hashCode = (hashCode * 397) ^ SideCount;
            hashCode = (hashCode * 397) ^ Modifier;
            return hashCode;
        }
#endif
    }



    private static readonly Regex DiceNotationRegex = new
    (
        @"^(?<dieCount>\d*)[dD](?<sideCount>\d+)(?<modifier>[+-]\d+)*$",
        RegexOptions.Compiled
    );



    /// <summary>
    /// Tries to parse a string representation of dice notation into a <see cref="Dice"/> instance.
    /// </summary>
    /// <param name="notation">The string representation of the dice notation.</param>
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

        var match = DiceNotationRegex.Match(notation);
        if (!match.Success)
        {
            return Result<Dice?>.Failure("Invalid dice notation format. Value must be in XdY+Z format.");
        }

        if (match.Groups["dieCount"].Value.Length > 9)
        {
            return Result<Dice?>.Failure("Die count value is out of range.");
        }


        // Get the die count and validate it
        var tryGetDieCountResult = TryGetDieCount(match.Groups["dieCount"].Value);
        if (tryGetDieCountResult.Failed)
        {
            return Result<Dice?>.Failure(tryGetDieCountResult.ErrorMessage!);
        }
        var dieCount = tryGetDieCountResult.Value;


        // Get the side count and validate it
        var getSideResult = TryGetSideCount(match.Groups["sideCount"].Value);
        if (getSideResult.Failed)
        {
            return Result<Dice?>.Failure(getSideResult.ErrorMessage!);
        }
        var sideCount = getSideResult.Value;


        // Get the modifier and validate it
        var getModifierResult = TryGetModifierTotal(match.Groups["modifier"].Captures);
        if (getModifierResult.Failed)
        {
            return Result<Dice?>.Failure(getModifierResult.ErrorMessage!);
        }
        var modifier = getModifierResult.Value;

        return Result<Dice?>.Success(new Dice(dieCount, sideCount, modifier));
    }



    private static Result<int> TryGetDieCount(string value)
    {

        // if not specified assume 1 i.e. "d6" is the same as "1d6"
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



    private static Result<int> TryGetModifierTotal(CaptureCollection modifiers)
    {
        var total = 0;

        foreach (Capture modifier in modifiers)
        {
            if (!int.TryParse(modifier.Value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var modifierValue))
            {
                return Result<int>.Failure("Modifier value is out of range.");
            }

            try
            {
                checked
                {
                    total += modifierValue;
                }
            }
            catch (OverflowException)
            {
                return Result<int>.Failure("Modifier value is out of range.");
            }
        }
        return Result<int>.Success(total);
    }
}
