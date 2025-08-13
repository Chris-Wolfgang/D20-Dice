# D20 Dice

## Installation
```bash
dotnet add package Wolfgang.D20Dice
```

## Usage
```csharp
using DndDice;

internal class Program
{
    private static void Main()
    {
        const int dieCount = 2;
        const int sideCount = 6;
        const int modifier = -2;

        var dice = new DndDice.Dice(dieCount, sideCount, modifier);
        Console.WriteLine($"Rolling {dieCount}d{sideCount}{(modifier != 0 ? (modifier > 0 ? "+" : "") + modifier : "")}:");
        Console.WriteLine($"Result: {dice.Roll()}");

    }
}
```
