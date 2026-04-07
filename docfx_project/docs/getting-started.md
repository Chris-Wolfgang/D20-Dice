# Getting Started

This guide will help you quickly get up and running with Wolfgang.D20-Dice.

## Prerequisites

- .NET 8.0 SDK or later (for development; the library targets .NET Framework 4.6.2+, .NET Standard 2.0, and .NET 8.0+)

## Installation

### Via NuGet Package Manager

```bash
dotnet add package Wolfgang.D20-Dice
```

### Via Package Manager Console

```powershell
Install-Package Wolfgang.D20-Dice
```

## Quick Start

```csharp
using Wolfgang.D20;

// Roll a single d20
var d20 = new Dice(dieCount: 1, sideCount: 20);
int result = d20.Roll();
Console.WriteLine($"Rolled {d20}: {result}");  // e.g. "Rolled 1d20: 14"

// Roll 2d6 with a +3 modifier
var attackRoll = new Dice(dieCount: 2, sideCount: 6, modifier: 3);
Console.WriteLine($"Attack ({attackRoll}): {attackRoll.Roll()}");
Console.WriteLine($"Range: {attackRoll.MinValue}–{attackRoll.MaxValue}");

// Parse dice notation from a string
var parseResult = Dice.TryParse("1d20+5");
if (parseResult.Succeeded)
{
    Console.WriteLine($"Parsed: {parseResult.Value} → {parseResult.Value.Roll()}");
}
```

## Next Steps

- Explore the [API Reference](https://chris-wolfgang.github.io/D20-Dice/versions/latest/api/Wolfgang.D20.html) for detailed documentation
- Read the [Introduction](introduction.md) to learn more about Wolfgang.D20-Dice
- Check out example projects in the [GitHub repository](https://github.com/Chris-Wolfgang/D20-Dice)

## Common Issues

### Parsing fails for valid-looking notation

`Dice.TryParse` expects the format `XdY`, `XdY+Z`, or `XdY-Z` where X is optional (defaults to 1). The die count must be at least 1 and the side count at least 2. Check `result.ErrorMessage` for details on why parsing failed.

### Modifier can be negative

A negative modifier is valid — `new Dice(1, 20, -2)` creates `1d20-2`. The `MinValue` property accounts for the modifier, so it may be less than 1.

## Additional Resources

- [GitHub Repository](https://github.com/Chris-Wolfgang/D20-Dice)
- [Contributing Guidelines](https://github.com/Chris-Wolfgang/D20-Dice/blob/main/CONTRIBUTING.md)
- [Report an Issue](https://github.com/Chris-Wolfgang/D20-Dice/issues)
