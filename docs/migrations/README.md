# Migration guides

Per-major-version upgrade paths for consumers of `Wolfgang.D20.Dice`.

## When a migration guide is required

**Every MAJOR release** (`1.x → 2.0`, `2.x → 3.0`, or `0.x → 1.0` when
the `1.0` cut carries a breaking change). Also warranted for a MINOR
that ships a large-surface deprecation via `[Obsolete]` even without
removals, so consumers can plan the eventual `MAJOR + 1`.

If a major version ships without a migration guide, consumers hit the
breaking change at runtime and reconstruct the fix from source diffs —
that is exactly the cost this convention exists to avoid.

## When to write it

**During release prep — same PR as the MAJOR bump**, NOT after. If the
migration guide isn't ready, the release isn't ready.

The guide's PR base branch is the same one that carries the MAJOR bump
(typically `vNext` or a per-cycle release branch, per the
per-repo-release-pilot skill).

## File naming

- `TEMPLATE-major-version-migration.md` — the template. Never edited
  during a release; use as-is by copying to the new file.
- `v<MAJOR>.md` — the migration guide (e.g. `v1.md`, `v2.md`). Named
  by the destination major-version. A `v2.md` migration guide
  describes the `1.x → 2.0` upgrade.

## Linking from release notes

The migration guide MUST be linked from the GitHub Release notes for
the MAJOR version. A single line at the top:

```markdown
**Upgrading from <PREV-MAJOR>.x?** See
[the v<MAJOR> migration guide](docs/migrations/v<MAJOR>.md).
```

## Related

- [`docs/adr/`](../adr/) — Architecture Decision Records document the
  *why* behind non-obvious current decisions. Migration guides document
  the *change* consumers experience across a MAJOR boundary. When a
  MAJOR removes a decision captured by an ADR, both the ADR and the
  migration guide need to reference each other.
- [`CHANGELOG.md`](../../CHANGELOG.md) — pre-1.0, a breaking change can
  ship in a MINOR (e.g. `0.7.x → 0.8.0` made `Dice` immutable). Those
  releases carry a migration note in their CHANGELOG section; a full
  guide here is optional until the first MAJOR.
