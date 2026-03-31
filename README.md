# Wolfgang.D20-Dice

A random number generator that simulates d20 dice with modifier

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Multi--Targeted-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github)](https://github.com/Chris-Wolfgang/D20-Dice)

---

## 📦 Installation

```bash
dotnet add package Wolfgang.D20-Dice
```

**NuGet Package:** Available on Nuget.org

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📚 Documentation

- **GitHub Repository:** [https://github.com/Chris-Wolfgang/D20-Dice](https://github.com/Chris-Wolfgang/D20-Dice)
- **API Documentation:** https://Chris-Wolfgang.github.io/D20-Dice/
- **Formatting Guide:** [README-FORMATTING.md](README-FORMATTING.md)
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
if (parseResult.IsSuccess)
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
if (result.IsSuccess)
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

| Framework | Versions |
|-----------|----------|
| .Net Framework | .net 4.6.2, .net 4.7.0, .net 4.7.1, .net 4.7.2, .net 4.8, .net 4.8.1 | 
| .Net Core | |
| .Net | .net 5.0, .net 6.0, .net 7.0, .net 8.0, .net 9.0, .net 10.0 |

---

## 🔍 Code Quality & Static Analysis

This project enforces **strict code quality standards** through **7 specialized analyzers** and custom async-first rules:

### Analyzers in Use

1. **Microsoft.CodeAnalysis.NetAnalyzers** - Built-in .NET analyzers for correctness and performance
2. **Roslynator.Analyzers** - Advanced refactoring and code quality rules
3. **AsyncFixer** - Async/await best practices and anti-pattern detection
4. **Microsoft.VisualStudio.Threading.Analyzers** - Thread safety and async patterns
5. **Microsoft.CodeAnalysis.BannedApiAnalyzers** - Prevents usage of banned synchronous APIs
6. **Meziantou.Analyzer** - Comprehensive code quality rules
7. **SonarAnalyzer.CSharp** - Industry-standard code analysis

### Async-First Enforcement

This library uses **`BannedSymbols.txt`** to prohibit synchronous APIs and enforce async-first patterns:

**Blocked APIs Include:**
- ❌ `Task.Wait()`, `Task.Result` - Use `await` instead
- ❌ `Thread.Sleep()` - Use `await Task.Delay()` instead
- ❌ Synchronous file I/O (`File.ReadAllText`) - Use async versions
- ❌ Synchronous stream operations - Use `ReadAsync()`, `WriteAsync()`
- ❌ `Parallel.For/ForEach` - Use `Task.WhenAll()` or `Parallel.ForEachAsync()`
- ❌ Obsolete APIs (`WebClient`, `BinaryFormatter`)

**Why?** To ensure all code is **truly async** and **non-blocking** for optimal performance in async contexts.

---

## 🛠️ Building from Source

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later
- Optional: [PowerShell Core](https://github.com/PowerShell/PowerShell) for formatting scripts

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
pwsh ./format.ps1
```

### Code Formatting

This project uses `.editorconfig` and `dotnet format`:

```bash
# Format code
dotnet format

# Verify formatting (as CI does)
dotnet format --verify-no-changes
```

See [README-FORMATTING.md](README-FORMATTING.md) for detailed formatting guidelines.

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
