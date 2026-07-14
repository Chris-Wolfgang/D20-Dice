# Architecture Decision Records

This directory records the significant architecture / design decisions for
**D20-Dice**, using lightweight [Architecture Decision Records (ADRs)](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions).

An ADR captures a single decision: the context that forced it, the decision
itself, and the consequences (good and bad). ADRs are immutable once accepted —
when a decision changes, add a **new** ADR that supersedes the old one rather
than editing history.

## Index

| ADR | Title | Status |
|-----|-------|--------|
| [0001](0001-record-architecture-decisions.md) | Record architecture decisions | Accepted |
| [0002](0002-dice-is-a-mutable-collection-of-die.md) | `Dice` is a mutable collection of `Die` | Accepted |
| [0003](0003-allocation-free-roll-hot-path.md) | Allocation-free `Roll` hot path | Accepted |
| [0004](0004-invariant-culture-dice-notation.md) | Invariant-culture dice notation | Accepted |
| [0005](0005-aot-compatibility-via-analyzers.md) | Verify AOT compatibility via analyzers | Accepted |
| [0006](0006-api-compat-gate-via-packagevalidation.md) | API/ABI compatibility gate via PackageValidation | Accepted |
| [0007](0007-mutation-testing-quality-bar.md) | Mutation-testing quality bar (Stryker) | Accepted |

## Adding an ADR

1. Copy the format of an existing record (Nygard style: Title, Status, Context,
   Decision, Consequences).
2. Number it with the next free 4-digit sequence.
3. Set the status to `Proposed`, then `Accepted` once agreed (or `Superseded by ADR-NNNN`).
4. Add a row to the index table above.
