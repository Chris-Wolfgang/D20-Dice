# Copilot Instructions for Wolfgang.D20.Dice

## Project Overview
- **Package:** Wolfgang.D20.Dice
- **Namespace:** Wolfgang.D20
- **Purpose:** Simulates tabletop dice rolls using standard dice notation (`XdY+Z`), including heterogeneous pools such as `2d6+1d4+3`
- **Dependency:** Wolfgang.TryPattern, for the `Result<T>` that `Dice.TryParse` returns

## Key Types
The authoritative surface is `src/Wolfgang.D20.Dice/PublicAPI.Shipped.txt` (+ `PublicAPI.Unshipped.txt` for what the next release adds).

- `Die` — `sealed`, implements `IDie`, `IEquatable<Die>`. One die: `SideCount` (at least 2), `MinValue` (always 1), `MaxValue`, `Roll()`, `ToString()` (`d6`). Value equality on `SideCount`.
- `Dice` — `sealed partial`, implements `IDice`, `IReadOnlyCollection<Die>`, `IEquatable<Dice>`. An **immutable** pool of `Die` plus a flat `Modifier` (ADR-0008): `DieCount` / `Count`, `MinValue`, `MaxValue`, `Roll()`, `ToString()`, the builders `WithDie` / `Without` / `WithModifier` (each returns a new instance), `static TryParse(string?)`, and `static MaxDieCount` (10,000; ADR-0009). Equality and hashing are order-independent over the multiset of dice plus the modifier, so a `Dice` is a safe dictionary key.
- `IDice`, `IDie` — the read-only contracts of the two types above. `IDice` has no `SideCount` (removed in 0.6.0, when pools became heterogeneous) and `Dice` no longer implements `IEqualityComparer<Dice>` (also 0.6.0).
- `AverageRollExtensions` — `AverageRoundedUp()` / `AverageRoundedDown()` on `IDie` and `IDice`.
- `Result<T>` — from Wolfgang.TryPattern. `Dice.TryParse` returns `Result<Dice?>`: check `Succeeded` / `Failed`, then `Value` (non-null on success) or `ErrorMessage` (non-null on failure, `null` on success since TryPattern 0.4).

## Code Style
- Allman brace style; 3 blank lines between members; `var` for obvious types; file-scoped namespaces
- Explicit `using` directives in every file: implicit/global usings are off on every project (#343)
- Null checks use the `is null` pattern
- Release builds treat every warning as an error, including missing XML docs on public members (CS1591) and the trim / Native AOT analyzers on `net8.0` / `net10.0`
- A new or changed public member must be recorded in `PublicAPI.Unshipped.txt` (PublicApiAnalyzers RS0016 / RS0017)
- `Roll` and `MaxValue` are allocation-free and LINQ-free hot paths (ADR-0003); `ToString` and `Equals` are deliberately exempt
- Dice notation is always formatted and parsed with `CultureInfo.InvariantCulture` (ADR-0004)
- Tests are named `MethodUnderTest_when_condition_expected_result`, use xunit 2.9.3, and run on every TFM from `net462` to `net10.0`
- A PR that changes `src/` adds one fragment under `changelog/unreleased/` (`type: breaking|feature|fix|docs|internal` + one sentence) or carries the `no-changelog` label; `CHANGELOG.md` is assembled at release time

## Target Frameworks
- `net462`, `netstandard2.0`, `net8.0`, `net10.0`
- `#if NET6_0_OR_GREATER` selects `Random.Shared` in `Die.Roll` (the legacy targets share one `Random` under a lock); `#if NET7_0_OR_GREATER` selects the `[GeneratedRegex]` tokeniser in `Dice.TryParse` (the legacy targets use a compiled `Regex`). There are no other framework-specific branches.
