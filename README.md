# Wolfgang.D20.Dice

A random number generator that simulates dice rolls using standard `XdY+Z` notation (e.g., `2d6+3`, `1d20-1`).

[![NuGet](https://img.shields.io/nuget/v/Wolfgang.D20.Dice.svg?logo=nuget&label=NuGet)](https://www.nuget.org/packages/Wolfgang.D20.Dice)
[![NuGet downloads](https://img.shields.io/nuget/dt/Wolfgang.D20.Dice.svg?logo=nuget&label=downloads)](https://www.nuget.org/packages/Wolfgang.D20.Dice)
[![PR build](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/D20-Dice/pr.yaml?event=pull_request_target&label=PR%20build&logo=github)](https://github.com/Chris-Wolfgang/D20-Dice/actions/workflows/pr.yaml)
[![Release](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/D20-Dice/release.yaml?label=release&logo=github)](https://github.com/Chris-Wolfgang/D20-Dice/actions/workflows/release.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Multi--Targeted-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github)](https://github.com/Chris-Wolfgang/D20-Dice)

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
    Console.WriteLine($"Parsed: {parseResult.Value} → {parseResult.Value.Roll()}");
}
```

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| Dice Notation | Standard `XdY+Z` format (e.g., `2d6+3`) |
| Parsing | `Dice.TryParse("1d20+5")` with full validation via `Result<T>` |
| Roll | `Roll()` generates a random result within the valid range |
| Min/Max | `MinValue` and `MaxValue` computed from dice configuration |
| Modifiers | Positive, negative, or zero modifiers supported |
| Equality | Full `IEquatable<Dice>` and `IEqualityComparer<Dice>` support |
| ToString | Formats back to dice notation (`2d6+3`, `1d20`, `3d8-2`) |
| Multi-TFM | Targets .NET Framework 4.6.2+, .NET Standard 2.0, and .NET 5.0–10.0 |

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
    var dice = result.Value;
    Console.WriteLine($"{dice} range: {dice.MinValue}–{dice.MaxValue}");
    Console.WriteLine($"Rolled: {dice.Roll()}");
}

// Equality comparison
var a = new Dice(2, 6, 3);
var b = new Dice(2, 6, 3);
Console.WriteLine(a.Equals(b));  // True
```

---

## 🎯 Target Frameworks

The shipped library targets four TFMs:

| Family | Targets |
|---|---|
| .NET Framework | `net462` |
| .NET Standard | `netstandard2.0` |
| Modern .NET | `net8.0`, `net10.0` |

`netstandard2.0` lets the package be consumed on `net47/471/472/48/481` and `net5.0`–`net9.0` consumers without the library shipping per-TFM assemblies for each. The test project multi-targets the full `net462`–`net10.0` matrix to verify behaviour end-to-end.

---

## 🔍 Code Quality & Static Analysis

This project is held to the canonical analyzer set used across all `Chris-Wolfgang` .NET libraries. Analyzers run on every build and are treated as errors in `Release`.

### Analyzers in Use

1. **Microsoft.CodeAnalysis.NetAnalyzers** — built-in .NET analyzers (correctness and performance)
2. **Roslynator.Analyzers** — refactoring and code quality
3. **AsyncFixer** — async/await best practices
4. **Microsoft.VisualStudio.Threading.Analyzers** — thread safety
5. **Microsoft.CodeAnalysis.BannedApiAnalyzers** — enforces the `BannedSymbols.txt` policy
6. **Meziantou.Analyzer** — broad code-quality rules
7. **SonarAnalyzer.CSharp** — industry-standard analysis
8. **Microsoft.CodeAnalysis.PublicApiAnalyzers** — scaffolded. Activates when `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` files are present alongside a src csproj; the baseline for this repo will be added in a follow-up using the IDE "Add to public API" code fix.

### Banned-API policy

`BannedSymbols.txt` is the canonical fleet baseline. The same policy is applied to every Wolfgang.* library — including this one, even though `Dice` itself exposes a synchronous API surface. Banned categories include blocking sync-over-async (`Task.Result`, `Task.Wait()`), `Thread.Sleep`, synchronous file I/O, and several legacy / deprecated APIs.

---

## 🛠️ Building from Source

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) to build the highest-targeted TFM
- Building the `net462` target also requires the .NET Framework 4.6.2 reference assemblies / targeting pack (typically installed via Visual Studio's ".NET desktop development" workload on Windows; the SDK alone does not include them, and `net462` builds are Windows-only)
- The `netstandard2.0` and `net8.0` targets build with the .NET 10 SDK alone on any OS
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

This project uses [DocFX](https://dotnet.github.io/docfx/) to generate API documentation:

```bash
# Install DocFX (one-time setup)
dotnet tool install -g docfx

# Generate API metadata and build documentation
cd docfx_project
docfx metadata  # Extract API metadata from source code
docfx build     # Build HTML documentation

# Documentation is generated in the docs/ folder at the repository root
```

The documentation is automatically built and deployed to GitHub Pages when changes are pushed to the `main` branch.

**Local Preview:**
```bash
# Serve documentation locally (with live reload)
cd docfx_project
docfx build --serve

# Open http://localhost:8080 in your browser
```

**Documentation Structure:**
- `docfx_project/` - DocFX configuration and source files
- `docs/` - Generated HTML documentation (published to GitHub Pages)
- `docfx_project/index.md` - Main landing page content
- `docfx_project/docs/` - Additional documentation articles
- `docfx_project/api/` - Auto-generated API reference YAML files

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
