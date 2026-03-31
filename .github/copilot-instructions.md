# Copilot Instructions for Wolfgang.D20-Dice

## Project Overview
- **Package:** Wolfgang.D20-Dice
- **Namespace:** Wolfgang.D20
- **Purpose:** Simulates tabletop dice rolls using standard dice notation (XdY+Z)
- **Dependency:** Wolfgang.TryPattern for `Result<T>` return types

## Key Types
- `Dice` — main class implementing `IDice`, `IEquatable<Dice>`, `IEqualityComparer<Dice>`
- `IDice` — interface defining DieCount, SideCount, Modifier, MinValue, MaxValue, Roll(), ToString()
- `Result<T>` — from Wolfgang.TryPattern, used by `Dice.TryParse()`

## Code Style
- Allman brace style
- 3 blank lines between members
- `var` for obvious types
- File-scoped namespaces
- Null checks use `is null` pattern
- Warnings as errors in Release builds

## Target Frameworks
- net462, netstandard2.0, net8.0, net10.0
- Use `#if NET5_0_OR_GREATER` for framework-specific code (e.g., HashCode.Combine)
