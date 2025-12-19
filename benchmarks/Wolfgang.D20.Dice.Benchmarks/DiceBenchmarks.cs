using BenchmarkDotNet.Attributes;
using Wolfgang.D20;

namespace Wolfgang.D20.Benchmarks;

[MemoryDiagnoser]
public class DiceBenchmarks
{
    private Dice? _d20;
    private Dice? _d6;

    [GlobalSetup]
    public void Setup()
    {
        _d20 = new Dice(1, 20);
        _d6 = new Dice(1, 6);
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
}
