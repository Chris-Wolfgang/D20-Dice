#if NET6_0_OR_GREATER
using Xunit;

namespace Wolfgang.D20.Tests.Unit;

/// <summary>
/// Verifies that the roll hot paths allocate nothing on the managed heap. Guarded to net6.0+, where
/// <see cref="GC.GetAllocatedBytesForCurrentThread"/> is available and JIT behaviour is stable.
/// </summary>
public class AllocationTests
{

    private const int Iterations = 1000;



    /// <summary>
    /// Measures the bytes allocated on the current thread while running <paramref name="action"/>
    /// <see cref="Iterations"/> times, after a warm-up pass that lets the JIT compile the code under test.
    /// </summary>
    private static long MeasureAllocations(Action action)
    {
        // Warm up so method JIT / tiered compilation happens before measurement rather than during it.
        for (var i = 0; i < 100; i++)
        {
            action();
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < Iterations; i++)
        {
            action();
        }
        return GC.GetAllocatedBytesForCurrentThread() - before;
    }



    [Fact]
    public void Die_Roll_allocates_nothing()
    {
        // Arrange
        var die = new Die(20);

        // Act
        var allocated = MeasureAllocations(() => die.Roll());

        // Assert
        Assert.Equal(0, allocated);
    }



    [Fact]
    public void Dice_Roll_allocates_nothing()
    {
        // Arrange - a heterogeneous pool with a modifier exercises the full Roll path.
        var dice = new Dice(new[] { new Die(6), new Die(6), new Die(4) }, modifier: 2);

        // Act
        var allocated = MeasureAllocations(() => dice.Roll());

        // Assert
        Assert.Equal(0, allocated);
    }



    [Fact]
    public void Dice_MaxValue_allocates_nothing()
    {
        // Arrange
        var dice = new Dice(new[] { new Die(6), new Die(6), new Die(4) }, modifier: 2);

        // Act
        var allocated = MeasureAllocations(() => _ = dice.MaxValue);

        // Assert
        Assert.Equal(0, allocated);
    }

}
#endif
