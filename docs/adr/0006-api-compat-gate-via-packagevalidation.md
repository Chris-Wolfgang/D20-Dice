# 6. API/ABI compatibility gate via PackageValidation

## Status

Accepted

## Context

As a published package, D20-Dice must avoid shipping an accidental breaking
change (a removed or changed public member, or an ABI break across target
frameworks). The `Microsoft.DotNet.ApiCompat` engine can diff the packed API
surface against a baseline; it is also available SDK-integrated as
PackageValidation, with no separate tool or workflow.

## Decision

Enable the SDK's `EnablePackageValidation` with `PackageValidationBaselineVersion`
set to the last released version. At `dotnet pack` time (including in
`release.yaml`) the packed surface is validated against the baseline package from
NuGet; an accidental break fails the pack with a `CP0xxx` error (issue #169).

## Consequences

- Breaking changes are caught before a release, wherever `dotnet pack` runs.
- The baseline version must be bumped to the newly released version as part of
  each release (a companion to the `<Version>` bump).
- Intentional breaks can be waived with an `ApiCompatSuppressionFile`.
