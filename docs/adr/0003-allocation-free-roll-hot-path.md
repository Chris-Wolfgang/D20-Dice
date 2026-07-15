# 3. Allocation-free `Roll` hot path

## Status

Accepted

## Context

`Dice.Roll()` (and `Dice.MaxValue`) are the hot path — a simulation or game loop
may call `Roll()` millions of times. The original implementation summed via
`Enumerable.Sum(_dice, ...)`. Calling a LINQ operator on the `List<Die>` field
through `IEnumerable<Die>` boxes the list's struct enumerator on the heap, so
every call allocated.

## Decision

Iterate the concrete `List<Die>` directly with `foreach`, which uses the list's
struct enumerator and allocates nothing. The `checked` context is preserved so
overflow still throws. Allocation is guarded by tests using
`GC.GetAllocatedBytesForCurrentThread` (net6.0+) asserting `Die.Roll`,
`Dice.Roll`, and `Dice.MaxValue` allocate zero bytes (see issue #179).

## Consequences

- The roll hot path is allocation-free and GC-pressure-free.
- New code on this path must avoid LINQ and other hidden allocations; the
  allocation guard tests will fail if that regresses.
- Slightly more verbose than the LINQ one-liner — an intentional trade of brevity
  for zero allocation.
