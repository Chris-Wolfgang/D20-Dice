using BenchmarkDotNet.Attributes;
// ReSharper disable RedundantArgumentDefaultValue — explicit `Dice(1, 20)` / `Dice(1, 6)` names the benchmark scenario.

namespace Wolfgang.D20.Benchmarks;

[MemoryDiagnoser]
public class DiceBenchmarks
{
    // The notation parsed by TryParse and produced by ToString: heterogeneous with a modifier, so every
    // branch of the tokeniser and the formatter is on the measured path.
    private const string Notation = "2d6+1d4+3";

    private Dice? _d20;
    private Dice? _d6;
    private Dice? _heterogeneous;
    private Dice? _heterogeneousEqual;

    [GlobalSetup]
    public void Setup()
    {
        _d20 = new Dice(1, 20);
        _d6 = new Dice(1, 6);
        _heterogeneous = new Dice(new[] { new Die(6), new Die(6), new Die(4) }, 3);
        // An equal pool built in a different order, so Equals has to compare the multisets.
        _heterogeneousEqual = new Dice(new[] { new Die(4), new Die(6), new Die(6) }, 3);
    }

    [Benchmark]
    public int RollD20()
    {
        return _d20!.Roll();
    }

    [Benchmark]
    public int RollD6()
    {
        return _d6!.Roll();
    }

    [Benchmark]
    public int RollHeterogeneous()
    {
        return _heterogeneous!.Roll();
    }

    [Benchmark]
    public bool TryParse()
    {
        return Dice.TryParse(Notation).Succeeded;
    }

    [Benchmark]
    public string ToStringNotation()
    {
        return _heterogeneous!.ToString();
    }

    [Benchmark]
    public bool EqualsHeterogeneous()
    {
        return _heterogeneous!.Equals(_heterogeneousEqual);
    }

    [Benchmark]
    public int GetHashCodeHeterogeneous()
    {
        return _heterogeneous!.GetHashCode();
    }
}
