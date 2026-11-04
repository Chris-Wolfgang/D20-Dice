# Wolfgang.D20.Dice

A random number generator that simulates dice rolls using standard `XdY+Z` notation (e.g., `2d6+3`, `1d20-1`).

[![NuGet](https://img.shields.io/nuget/v/Wolfgang.D20.Dice.svg?logo=nuget&label=NuGet)](https://www.nuget.org/packages/Wolfgang.D20.Dice)
[![NuGet downloads](https://img.shields.io/nuget/dt/Wolfgang.D20.Dice.svg?logo=nuget&label=downloads)](https://www.nuget.org/packages/Wolfgang.D20.Dice)
[![Build](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/D20-Dice/pr.yaml?branch=main&label=build&logo=github)](https://github.com/Chris-Wolfgang/D20-Dice/actions/workflows/pr.yaml)
[![Release](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/D20-Dice/release.yaml?label=release&logo=github)](https://github.com/Chris-Wolfgang/D20-Dice/actions/workflows/release.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Multi--Targeted-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github)](https://github.com/Chris-Wolfgang/D20-Dice)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/Chris-Wolfgang/D20-Dice/badge)](https://scorecard.dev/viewer/?uri=github.com/Chris-Wolfgang/D20-Dice)

---

## 📦 Installation

```bash
dotnet add package Wolfgang.D20.Dice
```

**NuGet Package:** [Wolfgang.D20.Dice](https://www.nuget.org/packages/Wolfgang.D20.Dice)

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📚 Documentation

- **GitHub Repository:** [https://github.com/Chris-Wolfgang/D20-Dice](https://github.com/Chris-Wolfgang/D20-Dice)
- **API Documentation:** https://Chris-Wolfgang.github.io/D20-Dice/
- **Formatting Guide:** [README-FORMATTING.md](docs/README-FORMATTING.md)
- **Contributing Guide:** [CONTRIBUTING.md](CONTRIBUTING.md)
- **Reproducible Builds:** [REPRODUCIBLE-BUILD.md](REPRODUCIBLE-BUILD.md)

---

## 🚀 Quick Start

```csharp
using Wolfgang.D20;

// Roll a single d20
var d20 = new Dice(dieCount: 1, sideCount: 20);
int result = d20.Roll();
Console.WriteLine($"Rolled {d20}: {result}");  // e.g. "Rolled 1d20: 14"

// Roll 2d6 with a +3 modifier
var attackRoll = new Dice(dieCount: 2, sideCount: 6, modifier: 3);
Console.WriteLine($"Attack ({attackRoll}): {attackRoll.Roll()}");  // e.g. "Attack (2d6+3): 11"
Console.WriteLine($"Range: {attackRoll.MinValue}–{attackRoll.MaxValue}");  // "Range: 5–15"

// Parse dice notation from a string
var parseResult = Dice.TryParse("1d20+5");
if (parseResult.Succeeded)
{
    // Value is Dice? (null only on failure), so it is safe to dereference once Succeeded is true.
    Console.WriteLine($"Parsed: {parseResult.Value} → {parseResult.Value!.Roll()}");
}

// Heterogeneous dice — Dice is an immutable value; the "with" builders return a new pool
var pool = Dice.TryParse("2d6+1d4+3").Value!;  // mixed dice plus a flat modifier
var bigger = pool.WithDie(new Die(8));         // returns a new pool with a d8 added (pool is unchanged)
Console.WriteLine($"{bigger}: {bigger.Roll()}"); // e.g. "2d6+1d4+1d8+3: 19"
```

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| Dice Notation | Standard `XdY+Z` format, including heterogeneous pools (e.g., `2d6+1d4+3`) |
| Heterogeneous dice | `Dice` is an immutable `IReadOnlyCollection<Die>` of any side count; `WithDie`/`Without` return a new pool |
| Parsing | `Dice.TryParse("2d6+1d4+3")` with full validation via `Result<T>` |
| Roll | `Roll()` generates a random result within the valid range |
| Min/Max | `MinValue` and `MaxValue` computed from the dice in the pool and the modifier |
| Modifiers | Positive, negative, or zero flat modifiers supported |
| Equality | Full `IEquatable<Dice>` / `IEquatable<Die>` support (dice pools compare order-independently) |
| ToString | Formats back to dice notation (`2d6+1d4+3`, `1d20`, `3d8-2`) |
| Multi-TFM | Targets .NET Framework 4.6.2, .NET Standard 2.0, .NET 8.0 and .NET 10.0 |

**Examples:**

```csharp
// Standard dice
var d20 = new Dice(1, 20);           // 1d20
var d6 = new Dice(2, 6, 3);          // 2d6+3
var penalty = new Dice(1, 8, -2);    // 1d8-2

// Parse from string notation
var result = Dice.TryParse("4d6+1");
if (result.Succeeded)
{
    var dice = result.Value!;
    Console.WriteLine($"{dice} range: {dice.MinValue}–{dice.MaxValue}");
    Console.WriteLine($"Rolled: {dice.Roll()}");
}

// Equality comparison
var a = new Dice(2, 6, 3);
var b = new Dice(2, 6, 3);
Console.WriteLine(a.Equals(b));  // True
```

---

## 🎯 Supported Frameworks

This library targets:

- **.NET Framework:** 4.6.2
- **.NET Standard:** 2.0
- **.NET:** 8.0, 10.0

See the [NuGet package page](https://www.nuget.org/packages/Wolfgang.D20.Dice/) for the authoritative per-TFM compatibility matrix.

## 🔍 Code Quality & Static Analysis

This project is held to the canonical analyzer set used across all `Chris-Wolfgang` .NET libraries. Analyzers run on every build and are treated as errors in `Release`.

### Analyzers in Use

Seven analyzers run unconditionally; the eighth, `PublicApiAnalyzers`, activates on a project that carries the `PublicAPI.*.txt` baseline files (`CONTRIBUTING.md` describes the seven).

1. **Microsoft.CodeAnalysis.NetAnalyzers** — built-in .NET analyzers (correctness and performance)
2. **Roslynator.Analyzers** — refactoring and code quality
3. **AsyncFixer** — async/await best practices
4. **Microsoft.VisualStudio.Threading.Analyzers** — thread safety
5. **Microsoft.CodeAnalysis.BannedApiAnalyzers** — enforces the `BannedSymbols.txt` policy
6. **Meziantou.Analyzer** — broad code-quality rules
7. **SonarAnalyzer.CSharp** — industry-standard analysis
8. **Microsoft.CodeAnalysis.PublicApiAnalyzers** — active since 0.9.0 (#262 / #272). The baseline lives in `src/Wolfgang.D20.Dice/PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt`; a new public member must be added to `PublicAPI.Unshipped.txt` (the IDE's "Add to public API" code fix does it) or the build fails with RS0016.

### Banned-API policy

`BannedSymbols.txt` is the canonical fleet baseline. The same policy is applied to every Wolfgang.* library — including this one, even though `Dice` itself exposes a synchronous API surface. Banned categories include blocking sync-over-async (`Task.Result`, `Task.Wait()`), `Thread.Sleep`, synchronous file I/O, and several legacy / deprecated APIs.

---

## 🔐 Verify the build

You do not have to take our word for what is in a published package.

- **Rebuild it yourself.** Every commit is verified to be *reproducible*, not merely deterministic:
  CI builds it on Ubuntu and on Windows and fails if any assembly or PDB differs.
  [REPRODUCIBLE-BUILD.md](REPRODUCIBLE-BUILD.md) has the step-by-step procedure for checking a
  released package against its tag, including which differences are expected and which are worth
  reporting. If your rebuild matches, you can record it publicly as a `Verification: v<version>`
  issue ([how](REPRODUCIBLE-BUILD.md#publish-your-verification)).
- **Check where it came from.** Each release carries a SLSA build-provenance attestation you can
  verify with `gh attestation verify`. See [Verifying a release](SECURITY.md#verifying-a-release)
  in the security policy.

---

## 🛠️ Building from Source

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) to build the highest-targeted TFM
- Building the `net462` target also requires the .NET Framework 4.6.2 reference assemblies / targeting pack (typically installed via Visual Studio's ".NET desktop development" workload on Windows; the SDK alone does not include them, and `net462` builds are Windows-only)
- The `netstandard2.0`, `net8.0` and `net10.0` targets build with the .NET 10 SDK alone on any OS
- Optional: [PowerShell Core](https://github.com/PowerShell/PowerShell) for `scripts/format.ps1`

### Build Steps

```bash
# Clone the repository
git clone https://github.com/Chris-Wolfgang/D20-Dice.git
cd D20-Dice

# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Run tests
dotnet test --configuration Release

# Run code formatting (PowerShell Core)
pwsh ./scripts/format.ps1
```

### Code Formatting

This project uses `.editorconfig` and `dotnet format`:

```bash
# Format code
dotnet format

# Verify formatting (as CI does)
dotnet format --verify-no-changes
```

See [README-FORMATTING.md](docs/README-FORMATTING.md) for detailed formatting guidelines.

### Building Documentation

API documentation is generated with [DocFX](https://dotnet.github.io/docfx/), pinned as a local tool in `.config/dotnet-tools.json` so contributors build with the same version CI does:

```bash
# Restore the pinned tools (one-time per clone)
dotnet tool restore

# Generate API metadata and build the site
cd docfx_project
dotnet docfx metadata   # Extract API metadata from src/ into docfx_project/api/
dotnet docfx build      # Build the HTML site into docfx_project/_site/
```

`.github/workflows/docfx.yaml` builds and deploys the versioned site to GitHub Pages; `release.yaml` calls it for every published release, and it can be run on demand from the Actions tab.

**Local Preview:**
```bash
# Serve the site locally (with live reload)
cd docfx_project
dotnet docfx build --serve

# Open http://localhost:8080 in your browser
```

**Documentation Structure:**
- `docfx_project/` - DocFX configuration and article sources
- `docfx_project/index.md` - Main landing page content
- `docfx_project/docs/` - Additional documentation articles
- `docfx_project/api/` - API reference metadata written by `dotnet docfx metadata` (git-ignored)
- `docfx_project/_site/` - The generated site that GitHub Pages serves (git-ignored)
- `docs/` - Committed project documentation: architecture decision records (`docs/adr/`), migration guides (`docs/migrations/`), and the workflow and formatting notes

---

## 🤝 Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for:
- Code quality standards
- Build and test instructions
- Pull request guidelines
- Analyzer configuration details

---


## 🙏 Acknowledgments

This project was made possible thanks to:
- The .NET community and ecosystem, which provides the runtime and tooling used by this library.
- [DocFX](https://dotnet.github.io/docfx/) for documentation generation.
- [shields.io](https://shields.io/) for the badges used in this README.
- All contributors who report issues, suggest improvements, or submit pull requests.
