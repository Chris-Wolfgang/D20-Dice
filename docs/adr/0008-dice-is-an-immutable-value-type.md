# 8. `Dice` is an immutable value type

## Status

Accepted (supersedes [ADR-0002](0002-dice-is-a-mutable-collection-of-die.md))

## Context

[ADR-0002](0002-dice-is-a-mutable-collection-of-die.md) made `Dice` a mutable
`ICollection<Die>`: dice could be `Add`/`Remove`/`Clear`ed and `Modifier` was
settable. To keep `GetHashCode` consistent with `Equals`, the hash had to be
derived from that mutable state — which introduced the classic **mutable-key
hazard**: a `Dice` used as a key in a `Dictionary`/`HashSet` and then mutated
lands in the wrong bucket and breaks lookups. We documented that hazard on
`Dice.GetHashCode` (issue #214) rather than removing it. The same mutability also
made `Dice` unsafe to share across threads.

A dice pool is conceptually a **value** — "2d6+3" is an expression, not a
container you edit in place. Modelling it as one removes the hazard structurally
instead of documenting around it (issues #239, and #170 for thread-safety).

## Decision

We will make `Dice` **immutable**. Concretely:

- Remove the mutating surface: `Add(Die)`, `Remove(Die)`, `RemoveAt(int)`,
  `Clear()`, and the `Modifier` **setter**. `Dice` no longer implements
  `ICollection<Die>`.
- `Dice` implements `IReadOnlyCollection<Die>` (plus `IDice`, `IEquatable<Dice>`),
  so callers can still enumerate the dice and read `Count`, but cannot mutate.
- Construction remains via the existing constructors and `TryParse`.
- Provide non-mutating **"with" builders** that return a new instance:
  `WithDie(Die)`, `Without(Die)`, `WithModifier(int)`.
- `Roll`, `MinValue`, `MaxValue`, `ToString`, `TryParse`, and equality are
  unchanged in behaviour — but equality/hashing are now **stable** for the life
  of an instance, because all state is read-only.

This is a **breaking API change**. Because the library is pre-1.0, it ships in a
**MINOR** bump (0.7.x → 0.8.0) per SemVer's 0.x allowance, with a migration note;
`AssemblyVersion` stays pinned at `1.0.0.0`.

## Consequences

- `Dice` is now a safe `Dictionary`/`HashSet` key and safe to share across
  threads — the #214 mutable-key documentation and the
  `ReSharper disable NonReadonlyMemberInGetHashCode` suppression on `GetHashCode`
  are removed as no longer applicable.
- The SDK PackageValidation gate ([ADR-0006](0006-api-compat-gate-via-packagevalidation.md))
  correctly flags the removed members against the 0.7.2 baseline; this is an
  intentional break, waived with an `ApiCompatSuppressionFile`
  (`CompatibilitySuppressions.xml`) committed alongside the change.
- Consumers that built pools imperatively (`Add`/`Remove`/`Clear`) or set
  `Modifier` after construction must switch to the constructors, `TryParse`, or
  the with-builders. This is the first entry that needs a consumer migration note
  (issue #181).
- ADR-0002 is superseded and marked as such.
