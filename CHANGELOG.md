# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `AverageRoundedUp()` and `AverageRoundedDown()` extension methods on `IDie` and `IDice`,
  returning the average roll rounded to a whole number. See #49.

### Changed

- **#179** — `Dice.Roll()` and `Dice.MaxValue` now iterate the underlying dice list directly
  instead of via LINQ `Sum`, eliminating the boxed-enumerator heap allocation so the roll hot
  path is allocation-free. Added allocation guard tests (net6.0+).
- **#162 (partial)** — Internal: added property-based tests (CsCheck, net8.0+) asserting roll
  results stay within bounds and that `Dice.ToString` round-trips through `Dice.TryParse`. The
  continuous/scheduled fuzz-running harness from #162 remains a follow-up.

### Deprecated

### Removed

### Fixed

- **#177** — `Die.ToString()` and `Dice.ToString()` now format numbers with the invariant
  culture, so dice notation always uses ASCII digits and an ASCII `-` sign and round-trips
  through `Dice.TryParse` regardless of the current thread culture.

### Security

## [0.6.1] - 2026-07-06

### Changed

- Dependabot bump: dotnet-dependencies group (5 packages).
## [0.6.0] - 2026-06-19

`Dice` is now a collection of individual `Die`, enabling heterogeneous
rolls such as `2d6+1d4+3`. See #48.

### Added

- **#48** — `Die` type representing a single die (`SideCount`, `MinValue`,
  `MaxValue`, `Roll`, value equality) and its `IDie` interface. Construct a
  die directly from its side count, e.g. `new Die(20)`.
- **#48** — `Dice` is now an `ICollection<Die>`: individual dice can be
  added and removed (`Add`, `Remove`, `RemoveAt`, `Clear`, `Contains`,
  `CopyTo`, enumeration), enabling heterogeneous pools such as `2d6+1d4+3`.
- **#48** — `Dice(IEnumerable<Die>, int modifier = 0)` constructor for
  building a pool from existing dice.

### Changed

- **#48** — `Dice.TryParse` now parses heterogeneous notation
  (e.g. `2d6+1d4+3`) and ignores whitespace. `Dice.ToString` groups dice
  by side count in the order each side count first appears, with the flat
  modifier always last.
- **#48** — `Dice` equality now compares the multiset of dice plus the
  modifier (order-independent); `Dice.Modifier` is now settable.

### Removed

- **#48 (breaking)** — `Dice.SideCount` has been removed; side count now
  lives on `Die`. A pool can contain dice with differing side counts, so a
  single `SideCount` on `Dice` is no longer meaningful.
- **#48 (breaking)** — `Dice` no longer implements `IEqualityComparer<Dice>`.
- **#48 (breaking)** — `IDice.SideCount` has been removed from the interface.

## [0.5.1] - 2026-05-31

Canonical maintenance round + binding-stability fix. No public API or
runtime behavior change vs v0.5.0.

### Added

- **D8** — `verify-docs-build` job in `release.yaml` runs DocFX during
  the release pipeline before the NuGet push, so a docs build failure
  now blocks the package from shipping.
- **A1** — `PublicApiAnalyzers` scaffolding (analyzers activate when
  `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` are present
  alongside the src csproj).
- **CI3** — canonical NuGet package metadata: `Authors`, `Copyright`,
  `RepositoryType`, SourceLink, snupkg symbol packages, deterministic
  CI build flag, and `EmbedUntrackedSources` hoisted to
  `Directory.Build.props`.
- **T3** — Stryker mutation-testing workflow (`stryker.yaml`).
- **T1** — coverage report published to docs site.
- **S1** — CodeQL `security-extended` query pack.
- **D6** — versions.json preservation guard on the docs deploy.
- **D7** — docs build cache hygiene.
- **P2** — BenchmarkDotNet → gh-pages chart workflow.
- `docs/DOCFX-VERSION-PICKER.md` documenting the version picker.

### Changed

- **C1** — fleet-wide template-drift sync: workflow files (`pr.yaml`,
  `release.yaml`, `docfx.yaml`, `codeql.yaml`,
  `build-all-versions.yaml`, `stryker.yaml`), `.editorconfig`,
  `BannedSymbols.txt`, `Directory.Build.props`, `tests/.editorconfig`,
  and `.gitattributes` consolidated to the canonical baseline.
- **Nullable** — `<Nullable>enable</Nullable>` consolidated into
  `Directory.Build.props` (was per-csproj); per-project opt-out via
  override still supported.
- **CI2** — Dependabot `github-actions` ecosystem added.
- **D3** — repo scripts hardened (`Setup-Labels.ps1`,
  `Fix-BranchRuleset.ps1`).
- Internal: analyzer `PackageReference`s centralized in
  `Directory.Build.props`.

### Fixed

- **C4** — restored explicit `<AssemblyVersion>1.0.0.0</AssemblyVersion>`
  (4-part form, was `1.0.0`) and added a prerelease-safe `<FileVersion>`
  (regex-strip property function). Keeps the assembly's binding identity
  stable across the 0.x line; .NET Framework consumers do not need to
  add a binding redirect to upgrade to v0.5.1 from any earlier v0.x.
  See DateTime-Extensions v1.3.1 post-mortem for what happens when this
  pin is dropped and SDK-derived AssemblyVersion changes per release.

[Unreleased]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.6.1...HEAD
[0.6.1]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.6.0...v0.6.1
[0.6.0]: https://github.com/Chris-Wolfgang/D20-Dice/releases/tag/v0.6.0
