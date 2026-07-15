# 2. `Dice` is a mutable collection of `Die`

## Status

Accepted

## Context

A dice pool needs to represent heterogeneous rolls such as `2d6 + 1d4 + 3`. Two
shapes were possible: an immutable value type describing a fixed expression, or a
mutable collection that can be built up and modified after construction.

## Decision

`Dice` implements `ICollection<Die>`: individual `Die` can be added and removed,
and `Modifier` is settable. Equality compares the multiset of dice plus the
modifier (order-independent), and `GetHashCode` is derived from that same state so
it stays consistent with `Equals`.

## Consequences

- Callers can construct a pool imperatively (`Add`/`Remove`) or from a sequence.
- Because the hash is derived from mutable state, a `Dice` instance must **not**
  be mutated while it is used as a key in a hashed collection (`Dictionary`,
  `HashSet`) — the standard mutable-key hazard. This constraint is documented on
  `Dice.GetHashCode` (see issue #214); callers needing a stable key can snapshot
  via `ToString()`.
- An immutable "dice expression" value type was considered and deferred; it can be
  added later as a separate type if a real use case appears, without changing
  this one.
