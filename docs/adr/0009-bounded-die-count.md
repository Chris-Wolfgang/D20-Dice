# 9. Bounded die count

## Status

Accepted

## Context

A `Dice` pool stores one `Die` reference per die in a `List<Die>` and `Roll` loops
over them. Nothing bounded the count: `new Dice(int.MaxValue, 6)` or
`Dice.TryParse("2000000000d6")` tried to build a two-billion-element list, which
is an `OutOfMemoryException` or a multi-second stall from twelve characters of
input. `TryParse` is the natural entry point for user-supplied notation (chat
bots, configuration files), so this was a cheap denial of service against any
consumer that parses untrusted input (issue #383).

The alternative of storing the pool as run-length `(sideCount, count)` pairs
would make a large homogeneous pool cost O(1) memory, but `Roll` would still
loop `dieCount` times, `IReadOnlyCollection<Die>` enumeration would still have
to yield every die, and the hot path would change shape. A cap is needed either
way, and a cap alone closes the hole.

## Decision

We will cap a pool at **`Dice.MaxDieCount` = 10,000 dice**, enforced at every
construction path:

- `Dice(int dieCount, ...)` throws `ArgumentOutOfRangeException` above the cap.
- `Dice(IEnumerable<Die>, ...)` throws `ArgumentException` on the element that
  would exceed the cap, checked per element so an unbounded sequence is rejected
  at the limit rather than materialised first.
- `WithDie` throws `InvalidOperationException` when the pool is already full.
- `TryParse` fails with `"Die count must be at most 10000."` when the dice in the
  notation add up to more than the cap, counting across terms, so
  `"5000d6+5001d4"` is rejected even though each term is valid alone.

The number is exposed as a static property, not a `const`, so a consumer that
reads it always sees the library's current value rather than one inlined at its
own compile time.

10,000 is far beyond any tabletop pool (a 20th-level fireball is 20d6, the
largest common pools are tens of dice) while keeping the worst case to tens of
kilobytes and a few microseconds per `Roll`.

## Consequences

- Untrusted notation can no longer exhaust memory or stall the process; the
  worst a 12-character string can do is cost a failed `Result`.
- A pool above 10,000 dice, which previously constructed, now throws or fails to
  parse. This is a behaviour change for inputs that were already impractical;
  it ships in a minor release with a changelog entry.
- The limit is a documented part of the contract. Raising it is a non-breaking
  change; lowering it would break consumers and needs a new ADR.
- Tests pin the boundary on every path (`MaxDieCount`, `MaxDieCount + 1`, the
  cap reached across two terms, an infinite sequence).
