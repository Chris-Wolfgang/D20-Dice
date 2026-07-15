# 1. Record architecture decisions

## Status

Accepted

## Context

D20-Dice is a small but long-lived library that ships to NuGet and is maintained
across many release cycles, often by different contributors (and automated
agents). Several non-obvious design choices — mutability, allocation behaviour,
culture handling, AOT posture — are easy to accidentally undo in a later change
because the *reasoning* lives only in commit messages or a reviewer's memory.

## Decision

We will keep Architecture Decision Records in `docs/adr/`, one Markdown file per
decision, in the Nygard format (Context / Decision / Consequences). Records are
immutable once accepted; a changed decision is captured as a new, superseding ADR.

## Consequences

- The rationale behind load-bearing choices is discoverable next to the code.
- Reviewers can point at an ADR instead of re-litigating a settled decision.
- There is a small ongoing cost: a genuinely architectural change should come
  with an ADR, not just code.
