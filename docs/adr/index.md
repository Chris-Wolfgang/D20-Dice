# ADR Index

All Architecture Decision Records for **D20-Dice**, with current status. See
[`README.md`](README.md) for how ADRs work and [`TEMPLATE.md`](TEMPLATE.md) for
the format to copy when adding one.

| ADR | Title | Status |
|-----|-------|--------|
| [0001](0001-record-architecture-decisions.md) | Record architecture decisions | Accepted |
| [0002](0002-dice-is-a-mutable-collection-of-die.md) | `Dice` is a mutable collection of `Die` | Superseded by [0008](0008-dice-is-an-immutable-value-type.md) |
| [0003](0003-allocation-free-roll-hot-path.md) | Allocation-free `Roll` hot path | Accepted |
| [0004](0004-invariant-culture-dice-notation.md) | Invariant-culture dice notation | Accepted |
| [0005](0005-aot-compatibility-via-analyzers.md) | Verify AOT compatibility via analyzers | Accepted |
| [0006](0006-api-compat-gate-via-packagevalidation.md) | API/ABI compatibility gate via PackageValidation | Accepted |
| [0007](0007-mutation-testing-quality-bar.md) | Mutation-testing quality bar (Stryker) | Accepted |
| [0008](0008-dice-is-an-immutable-value-type.md) | `Dice` is an immutable value type | Accepted |
