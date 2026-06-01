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
  `BannedSymbols.txt`, `Directory.Build.props`,
  `tests/Directory.Build.props`, and `.gitattributes` consolidated to
  the canonical baseline.
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
