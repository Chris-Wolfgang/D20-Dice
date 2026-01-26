using System.Text.RegularExpressions;

namespace Wolfgang.D20;

/// <summary>
/// Represents a number of dice, each with the specified number of sides and an optional modifier.
/// </summary>
public class Dice : IDice, IEquatable<Dice>
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
    /// Creates a new instance of <see cref="Dice"/> from a string notation in the format "XdY+Z" where:
    /// X is the number of dice, Y is the number of sides on each die, and Z is an optional modifier.
    /// </summary>
    /// <param name="notation">The string representation of the dice</param>
    /// <exception cref="ArgumentException">The notation is not in the correct format</exception>
    public Dice(string notation)
    {
        if (string.IsNullOrWhiteSpace(notation))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(notation));
        }

        var regex = new Regex(@"^(?<dieCount>\d+)[dD](?<sideCount>\d+)(?<modifier>[+-]\d+)*$");


        var match = regex.Match(notation);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid dice notation format. Value must be in XdY+Z format", nameof(notation));
        }


        var dieCount = int.Parse(match.Groups["dieCount"].Value);
        var sideCount = int.Parse(match.Groups["sideCount"].Value);
        var modifier = match.Groups["modifier"].Value != ""
            ? int.Parse(match.Groups["modifier"].Value)
            : 0;

        var modifier = match.Groups["modifier"].Captures
            .Cast<Capture>()
            .Sum(capture => int.Parse(capture.Value));

        if (dieCount < 1)
        {
            throw new ArgumentException( "Die count must be greater than 0.", nameof(notation));
        }

        if (sideCount < 2)
        {
            throw new ArgumentException( "Side count must be greater than 2.", nameof(notation));
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
    public int MinValue => DieCount * 1 + Modifier;


    /// <summary>
    /// The maximum value that can be rolled with the specified dice and modifier.
    /// </summary>
    public int MaxValue => DieCount * SideCount + Modifier;



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
            < 0 => $"{value}{Modifier}"
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
    /// Generates a hash code for this instance based on its DieCount, SideCount, and Modifier.
    /// </summary>
    /// <returns>int</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = DieCount;
            hashCode = (hashCode * 397) ^ SideCount;
            hashCode = (hashCode * 397) ^ Modifier;
            return hashCode;
        }
    }


}
