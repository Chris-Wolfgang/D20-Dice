namespace Wolfgang.D20;

/// <summary>
/// Represents a single die with a fixed number of sides.
/// </summary>
public sealed class Die : IDie, IEquatable<Die>
{
#if !NET6_0_OR_GREATER
    private static readonly Random SharedRandom = new();
#endif



    /// <summary>
    /// Constructs a new instance of <see cref="Die"/> with the specified number of sides.
    /// </summary>
    /// <param name="sideCount">The number of sides on the die</param>
    /// <exception cref="ArgumentOutOfRangeException">sideCount is less than 2</exception>
    /// <remarks>sideCount of 2 represents a coin toss</remarks>
    public Die(int sideCount = 6)
    {
        if (sideCount < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(sideCount));
        }
        SideCount = sideCount;
    }



    /// <summary>
    /// The number of sides on the die. Must be at least 2.
    /// </summary>
    /// <remarks>
    /// A value of 2 represents a coin toss, 3 represents a three-sided die, etc.
    /// </remarks>
    public int SideCount { get; }



    /// <summary>
    /// The minimum value that can be rolled on the die. Always 1.
    /// </summary>
    public int MinValue => 1;



    /// <summary>
    /// The maximum value that can be rolled on the die. Equal to <see cref="SideCount"/>.
    /// </summary>
    public int MaxValue => SideCount;



    /// <summary>
    /// Rolls the die and returns the value rolled.
    /// </summary>
    /// <returns>
    /// A uniform random value in [<see cref="MinValue"/>, <see cref="MaxValue"/>] inclusive.
    /// </returns>
    public int Roll()
    {
#if NET6_0_OR_GREATER
        var random = Random.Shared;
#else
        var random = SharedRandom;
#endif
        return random.Next(1, SideCount + 1);
    }



    /// <summary>
    /// Returns a string representation of the die in the format "dY" where Y is the number of sides.
    /// </summary>
    /// <returns>The die in standard <c>dY</c> notation.</returns>
    public override string ToString()
    {
        return $"d{SideCount}";
    }



    /// <summary>
    /// Checks if this instance is equal to another instance of <see cref="Die"/>.
    /// </summary>
    /// <param name="other">
    /// The other instance of <see cref="Die"/> to compare with this instance.
    /// </param>
    /// <returns>
    /// True if both instances have the same <see cref="SideCount"/>; otherwise, false.
    /// </returns>
    public bool Equals(Die? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return SideCount == other.SideCount;
    }



    /// <summary>
    /// Checks if this instance is equal to another object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance. It can be null or of any type.
    /// </param>
    /// <returns>
    /// True if the object is a <see cref="Die"/> instance with the same <see cref="SideCount"/>; otherwise, false.
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

        return Equals((Die)obj);
    }



    /// <summary>
    /// Generates a hash code for this instance based on its <see cref="SideCount"/>.
    /// </summary>
    /// <returns>A hash code derived from <see cref="SideCount"/>.</returns>
    public override int GetHashCode()
    {
        return SideCount.GetHashCode();
    }
}
