# 4. Invariant-culture dice notation

## Status

Accepted

## Context

`Die.ToString()` and `Dice.ToString()` emit standard dice notation (e.g.
`2d6-1`), and `Dice.TryParse` reads it back. The parser reads with
`CultureInfo.InvariantCulture`, but the formatters originally used the current
thread culture (string interpolation / `StringBuilder.Append(int)`). `int`
formatting is ASCII in every culture, but the **negative sign** is not — some
cultures use a non-ASCII minus — so under those cultures a negative modifier
produced notation that would not round-trip through `TryParse` (see issue #177).

## Decision

Both `ToString` overrides format all numbers with `CultureInfo.InvariantCulture`,
so notation always uses ASCII digits and an ASCII `-`, independent of the current
thread culture. A culture-matrix test plus a deterministic hostile-negative-sign
test guard the round-trip.

## Consequences

- `ToString()` output is stable and always parseable, regardless of ambient culture.
- Any future notation formatting must also use the invariant culture — the
  globalization tests will catch a regression.
