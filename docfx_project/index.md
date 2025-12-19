# Wolfgang D20 Dice Documentation

Welcome to the Wolfgang D20 Dice library documentation!

## Overview

Wolfgang D20 Dice is a .NET library for generating random numbers by simulating rolling of dice with various number of sides.

## Key Features

- Simple dice rolling simulation
- Support for various dice types (d4, d6, d8, d10, d12, d20, d100)
- Cross-platform support (.NET Framework 4.6.2+, .NET Standard 2.0, .NET 8.0+, .NET 10.0)
- Easy to use API

## Getting Started

Check out the [API Documentation](api/index.md) to learn how to use the library.

## Quick Example

```csharp
using Wolfgang.D20;

// Create a dice instance
var dice = new Dice();

// Roll a d20
int result = dice.Roll(20);
```

## Resources

- [GitHub Repository](https://github.com/Chris-Wolfgang/D20-Dice)
- [NuGet Package](https://www.nuget.org/packages/Wolfgang.D20.Dice)
- [License](https://github.com/Chris-Wolfgang/D20-Dice/blob/main/LICENSE)
