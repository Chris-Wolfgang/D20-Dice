# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

### Changed

### Deprecated

### Removed

### Fixed

### Security

## [0.7.2] - 2026-07-16

Maintenance round — public-API documentation plus supply-chain and debug-tooling
verification. **No public API or runtime behavior change vs 0.7.1.**

### Added

- **#174** — runnable `<example>` blocks on the public API XML docs (`Die`, `Dice`,
  `IDie`/`IDice`, and the `AverageRoundedUp`/`AverageRoundedDown` extensions). Every
  snippet is compile-verified against the library.
- **#171** — SLSA build-provenance attestation for the published `.nupkg`
  (`actions/attest-build-provenance`, keyless via the release workflow's OIDC identity),
  binding each package's digest to the exact commit and workflow run that built it. A new
  "Verifying a release" section in `SECURITY.md` documents `gh attestation verify` and the
  CycloneDX SBOM. (NuGet author signing remains tracked in #171 — it needs a certificate.)
- **#176** — a SourceLink verification gate (`dotnet sourcelink test`) that fetches every
  PDB source document from GitHub and checksum-matches it, so consumer debug step-into
  cannot silently regress to decompiled source on a mis-tagged or mis-uploaded release.

## [0.7.1] - 2026-07-15

Maintenance round — repo hardening, documentation, and internal tests. **No public
API or runtime behavior change vs 0.7.0.**

### Added

- **#182** — Architecture Decision Records under `docs/adr/`, capturing this line's
  load-bearing design decisions (mutability, allocation-free roll, invariant-culture
  notation, AOT posture, API-compat gate, mutation-testing bar).
- **#183** — a "Release path & compromise scope" appendix in `SECURITY.md`.
- **#173** — snapshot / approval tests (Verify) pinning the dice-notation format.
- **#220** — a native-AOT publish-and-run smoke consumer (`aot-smoke/`) and its CI
  workflow, the runtime counterpart to the #175 `IsAotCompatible` analyzer gate.
- CI hardening workflows: **#185** Actions audit (actionlint + zizmor), **#164** Semgrep
  SAST, **#178** build-reproducibility verification, **#180** transitive-license
  allow-list gate, **#171** CycloneDX SBOM, **#184** OSSF Scorecard, **#186** per-PR
  performance-regression detection.
- **#199** — ReSharper InspectCode is now a required status check.

### Changed

- **#214** — documented on `Dice.GetHashCode()` that `Dice` is mutable and must not be
  mutated while used as a key in a hashed collection (snapshot via `ToString` for a
  stable key).

### Fixed

- **#224** — the documentation version picker no longer selects the `latest` alias on
  non-versioned pages (site root / regular docs pages).

## [0.7.0] - 2026-07-13

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
- **#169** — Internal: added an API/ABI compatibility gate via the SDK's ApiCompat-based
  PackageValidation, baselined against the last release (`PackageValidationBaselineVersion`), so an
  accidental breaking change fails `dotnet pack` (and therefore the release). Intentional breaks can
  be waived with an `ApiCompatSuppressionFile`.
- **#168** — Internal: the Stryker mutation-testing run now enforces a 90% break threshold — a
  change that drops the mutation score below 90% fails the run. The run is triggered manually and
  weekly (not per-PR, since mutation runs are slow).
- **#175** — Internal: enabled the trim/AOT/single-file analyzers (`IsAotCompatible`) on the
  net8.0/net10.0 builds, so an AOT-incompatible change fails the build. This is static
  analyzer validation; a `PublishAot` publish-and-run smoke consumer is tracked as a follow-up.

### Fixed

- **#177** — `Die.ToString()` and `Dice.ToString()` now format numbers with the invariant
  culture, so dice notation always uses ASCII digits and an ASCII `-` sign and round-trips
  through `Dice.TryParse` regardless of the current thread culture.

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

[Unreleased]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.7.2...HEAD
[0.7.2]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.7.1...v0.7.2
[0.7.1]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.7.0...v0.7.1
[0.7.0]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.6.1...v0.7.0
[0.6.1]: https://github.com/Chris-Wolfgang/D20-Dice/compare/v0.6.0...v0.6.1
[0.6.0]: https://github.com/Chris-Wolfgang/D20-Dice/releases/tag/v0.6.0
