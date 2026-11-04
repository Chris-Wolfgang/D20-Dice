# Reproducible Builds

`Wolfgang.D20.Dice` is built **deterministically**: compiling the same commit **with the
same SDK** produces byte-for-byte identical assemblies and symbols, on any operating
system and regardless of the machine or working directory. This lets anyone independently
confirm that a published package was built from the tagged source and contains no hidden
modifications.

## The guarantee

For a given commit built **with the same .NET SDK**, the following outputs are bit-for-bit
identical, **on Linux and Windows alike**:

- the compiled assembly `Wolfgang.D20.Dice.dll` (every target framework:
  `net462`, `netstandard2.0`, `net8.0`, `net10.0`),
- its debug symbols `Wolfgang.D20.Dice.pdb`.

> **The package files are not byte-reproducible.** The `.nupkg` / `.snupkg` are zip
> containers, and NuGet stamps every entry with the time the package was packed and writes
> platform-specific zip attributes and newlines into its own metadata (`.nuspec`,
> `.psmdcp`, `_rels/.rels`). Packing the same commit twice, even on one machine, gives two
> different package files. What is inside them is stable: the assemblies and symbols in
> every package are identical across runs and across operating systems. Compare those,
> not the package file itself (see below).

## How it is achieved

Determinism is configured in [`Directory.Build.props`](Directory.Build.props),
[`Directory.Build.targets`](Directory.Build.targets) and the library project:

| Setting | Effect |
| --- | --- |
| `Deterministic` (SDK default `true`) | Removes timestamps and other non-deterministic metadata from the assembly. |
| `ContinuousIntegrationBuild=true` (set when `CI=true`) | Enables `DeterministicSourcePaths`, so absolute source paths are replaced with stable, machine-independent tokens. |
| `Microsoft.SourceLink.GitHub` + `EmbedUntrackedSources` | Maps source paths to their canonical repository location, which is what makes the PDB reproducible across machines. |
| `NormalizeGeneratedLineEndings` target (`Directory.Build.targets`) | The SDK writes its generated sources (`*.AssemblyInfo.cs`, `*.AssemblyAttributes.cs`) with the OS newline, CRLF on Windows and LF on Linux. The PDB records each file's hash and embeds its text, so the PDB, and through its checksum and the module version id the DLL, differed per OS. The target rewrites them to LF before compilation. |
| `Microsoft.NETFramework.ReferenceAssemblies` package reference (`Wolfgang.D20.Dice.csproj`, `net462`) | The PDB records every referenced assembly's timestamp and module version id. Windows would otherwise compile against the machine's installed targeting pack and Linux against this NuGet package, which are different builds of the same reference assemblies. Referencing the package explicitly makes every OS use it. |

`ContinuousIntegrationBuild` + SourceLink path normalization removes the machine- and
directory-specific differences. The last two rows remove the two operating-system-specific
inputs that remained.

## Continuous verification

The [`Build Reproducibility`](.github/workflows/reproducible-build.yaml) workflow checks
reproducibility on every pull request and push to `main`:

1. It packs the library on **both** `ubuntu-latest` and `windows-latest` with
   `ContinuousIntegrationBuild=true`.
2. Each job records the `sha256sum` of every produced `.dll`, `.pdb`, `.nupkg` and
   `.snupkg`.
3. The final `Reproducible build` job (a required status check) diffs the two manifests
   for the `.dll` and `.pdb` files and **fails** if any hash differs. The package files
   are left out of the comparison because they embed the pack time (see above).

## Verify it yourself

You can reproduce a released build locally and compare it against the published package.
Replace `<version>` with the release you are checking.

```bash
# 1. Check out the exact tagged source.
git clone https://github.com/Chris-Wolfgang/D20-Dice.git
cd D20-Dice
git checkout v<version>

# 2. Pack deterministically (CI=true turns on ContinuousIntegrationBuild).
CI=true dotnet pack src/Wolfgang.D20.Dice/Wolfgang.D20.Dice.csproj \
  -c Release -p:ContinuousIntegrationBuild=true -p:Deterministic=true -o artifacts

# 3. Hash your locally built assembly (any TFM shown here).
sha256sum src/Wolfgang.D20.Dice/bin/Release/net8.0/Wolfgang.D20.Dice.dll

# 4. Download the published package from nuget.org and extract the same assembly to compare.
curl -sSL -o published.nupkg \
  https://www.nuget.org/api/v2/package/Wolfgang.D20.Dice/<version>
unzip -o published.nupkg -d published
sha256sum published/lib/net8.0/Wolfgang.D20.Dice.dll
```

If the two hashes match, the published assembly was built from exactly this source. The
same holds for every other target framework, and for the `.pdb` files in the `.snupkg`. Do
not compare the `.nupkg` file's own hash: it embeds the pack time, so it differs on every
build.

> **Note on the SDK version.** Reproducibility is guaranteed for a *fixed* .NET SDK. A
> different SDK / compiler version can legitimately emit different (but still
> deterministic) bytes. Use the SDK the release used: it is recorded in the release's
> `reproducible-build-manifest.json` asset (`toolchain.sdk`, see below). Releases made
> before that asset existed do not carry it; for those, `dotnet --version` in the
> release's workflow run log shows the exact patch.

### Compare whole packages entry by entry

The steps above check one assembly. To check everything a package ships, compare your package
with the published one entry by entry. The package *file* hashes never match (see above), but
every entry inside them should:

```bash
pwsh ./scripts/compare-package-entries.ps1 \
  -Mine ./artifacts/Wolfgang.D20.Dice.<version>.nupkg \
  -Published ./published.nupkg
```

The script exits 0 when every entry matches and 1 otherwise, listing the entries that differ. It
handles two entries specially, neither of which comes from the build:

| Entry | Handling |
| --- | --- |
| `.signature.p7s` | **Skipped.** nuget.org's repository signature, added after the build. Present in the downloaded package, absent from yours. |
| `*.psmdcp` | **Compared** under a stable key rather than by name: OPC core properties, whose *file name* is a GUID on some NuGet versions and the fixed `nuget.psmdcp` on others. |

Releases are packed on Windows. The assemblies and symbols match from any OS, but NuGet writes
OS-specific newlines into its own metadata entries (`.nuspec`, `.psmdcp`, `_rels/.rels`), so run
the whole-package comparison on Windows, or expect exactly those entries to differ.

### Or compare against the release manifest

Each release also carries `reproducible-build-manifest.json`, which records the toolchain
that built it and a SHA-256 for every entry inside every package it shipped. Download it and
check your rebuild against it without downloading anything from nuget.org:

```bash
gh release download v<version> --repo Chris-Wolfgang/D20-Dice \
  --pattern 'reproducible-build-manifest.json'
pwsh -c '(Get-Content reproducible-build-manifest.json | ConvertFrom-Json).toolchain'

pwsh ./scripts/compare-package-entries.ps1 \
  -Mine ./artifacts/Wolfgang.D20.Dice.<version>.nupkg \
  -Manifest ./reproducible-build-manifest.json
```

Two things to know before reading a difference as one:

- **`sha256IsReproducible` is `false` on each package's own file hash.** It is recorded for
  completeness. Do not compare it: the package file embeds the pack time.
- **The manifest does not list every entry.** The `.psmdcp` is omitted, because its entry name
  is not stable across NuGet versions, and `.signature.p7s` does not exist yet when the
  manifest is written. The manifest says both in its own `verification.entriesNotListed`.

The manifest is written by the same workflow run that produced the packages, so it is a
convenience, not independent evidence. If the question is whether *we* published what we said
we did, compare against nuget.org as above, or verify the provenance attestation, which is
signed ([SECURITY.md](SECURITY.md#verifying-a-release)).

## File a discrepancy

If any hash differs and you are using the same SDK version, that is a supply-chain signal
worth reporting. Please [open an issue](https://github.com/Chris-Wolfgang/D20-Dice/issues/new)
titled `Reproducible-build discrepancy: v<version>` and include:

- the release tag and your `dotnet --version`,
- your OS / architecture,
- the two hashes and which file they are for.

Discrepancies are treated as a security concern; see [SECURITY.md](SECURITY.md).
