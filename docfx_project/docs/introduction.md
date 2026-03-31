# Introduction

Welcome to Wolfgang.D20-Dice!

## Overview

Wolfgang.D20-Dice is a .NET library for simulating tabletop dice rolls using standard dice notation (e.g., `2d6+3`). It provides a strongly-typed `Dice` class with roll generation, notation parsing, and value-equality semantics.

## Key Features

- **Dice Notation** — standard `XdY+Z` format with any die count, side count, and modifier
- **Parsing** — `Dice.TryParse("1d20+5")` with full validation, returning a `Result<T>` from [Wolfgang.TryPattern](https://github.com/Chris-Wolfgang/Try-Pattern)
- **Roll Generation** — `Roll()` produces a random result within the valid range
- **Min/Max Calculation** — `MinValue` and `MaxValue` computed from the dice configuration
- **Equality** — full `IEquatable<Dice>` and `IEqualityComparer<Dice>` implementation
- **ToString** — formats back to dice notation (`2d6+3`, `1d20`, `3d8-2`)

## Getting Help

If you need help with Wolfgang.D20-Dice, please:

- Check the [Getting Started](getting-started.md) guide
- Review the [API Reference](https://chris-wolfgang.github.io/D20-Dice/versions/latest/api/Wolfgang.D20.html)
- Visit the [GitHub repository](https://github.com/Chris-Wolfgang/D20-Dice)
- Open an issue on [GitHub Issues](https://github.com/Chris-Wolfgang/D20-Dice/issues)
