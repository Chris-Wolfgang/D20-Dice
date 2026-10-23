# Introduction

Welcome to Wolfgang.D20.Dice!

## Overview

Wolfgang.D20.Dice is a .NET library for simulating tabletop dice rolls using standard dice notation (e.g., `2d6+1d4+3`). It provides a `Die` type for a single die and an immutable `Dice` pool with roll generation, notation parsing, and value-equality semantics.

## Key Features

- **Dice Notation** — standard `XdY+Z` format, including heterogeneous pools such as `2d6+1d4+3`
- **`Die` and `Dice`** — a single die, or an immutable pool of dice with a flat modifier; the `WithDie` / `Without` / `WithModifier` builders return a new pool rather than mutating the current one
- **Parsing** — `Dice.TryParse("2d6+1d4+3")` with full validation, returning a `Result<T>` from [Wolfgang.TryPattern](https://github.com/Chris-Wolfgang/Try-Pattern); a pool holds at most `Dice.MaxDieCount` (10,000) dice
- **Roll Generation** — `Roll()` produces a random result within the valid range and is safe to call from any thread on every target framework
- **Min/Max and Averages** — `MinValue` and `MaxValue` computed from the dice and the modifier, plus the `AverageRoundedUp()` / `AverageRoundedDown()` extension methods
- **Equality** — `IEquatable<Dice>` and `IEquatable<Die>`; pools compare order-independently, so a `Dice` is a safe dictionary key
- **ToString** — formats back to dice notation (`2d6+1d4+3`, `1d20`, `3d8-2`), always culture-invariant

## Getting Help

If you need help with Wolfgang.D20.Dice, please:

- Check the [Getting Started](getting-started.md) guide
- Review the [API Reference](https://chris-wolfgang.github.io/D20-Dice/versions/latest/api/Wolfgang.D20.html)
- Visit the [GitHub repository](https://github.com/Chris-Wolfgang/D20-Dice)
- Open an issue on [GitHub Issues](https://github.com/Chris-Wolfgang/D20-Dice/issues)
