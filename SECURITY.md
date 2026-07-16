# Security Policy

## Reporting a Vulnerability

If you discover a security vulnerability, please follow these steps:

1. **Do not** create a public issue on this repository.
2. In the top navigation of this repository, click the **Security** tab.
3. In the top right, click the **Report a vulnerability** button.
4. Fill out the provided form with:
   - A description of the vulnerability
   - Steps to reproduce the issue
   - Potential impact
   - Suggested fix (if you have one)

## Response Timeline

We will acknowledge your report within 48 hours and provide an estimated timeline for a fix.

## Thank You

Your help is greatly appreciated!
Responsible disclosure of security vulnerabilities helps protect our entire community.

## Release path & compromise scope

Facts a maintainer would need at 2am if the release identity is compromised. Generic incident-response steps (rotating credentials, revoking OAuth apps, publishing advisories, unlisting NuGet packages) are not duplicated here — GitHub's and NuGet's own docs update faster than a checked-in runbook. The fleet-canonical full runbook is tracked at [Chris-Wolfgang/repo-template#430](https://github.com/Chris-Wolfgang/repo-template/issues/430).

- **Release path**: OIDC / NuGet **Trusted Publishing** via `NuGet/login@v1` in
  `.github/workflows/release.yaml` (triggered on `release: published`). The workflow mints an
  ephemeral push token per run via OIDC — the release path does **not** depend on a long-lived
  `NUGET_API_KEY` in GitHub secrets or on the NuGet account. During an incident, check the NuGet
  account for any long-lived API keys anyway (they can be created outside CI) and delete anything
  you don't recognize.
- **Fallback**: none. If Trusted Publishing is compromised, the incident is at the GitHub-account
  level (the OIDC identity is `Chris-Wolfgang/D20-Dice`).
- **Owner**: @Chris-Wolfgang.
- **Downstream consumers**: none known inside the Wolfgang.* org at time of writing; the package is public on nuget.org, so unknown downstream consumers may exist. Re-check via `dotnet-outdated`, GitHub code-search, and the NuGet package's `Used By` dependents list before communicating during an incident.
- **Package coordinates for unlisting**: `Wolfgang.D20.Dice` on nuget.org — <https://www.nuget.org/packages/Wolfgang.D20.Dice/>.

## Verifying a release

Every published release carries supply-chain evidence so consumers can prove a package
was built from this repository and not tampered with:

- **SLSA build provenance** — `release.yaml` generates a signed [SLSA](https://slsa.dev)
  provenance attestation for each `.nupkg` via `actions/attest-build-provenance`, using the
  workflow's OIDC identity (`Chris-Wolfgang/D20-Dice`). The attestation binds the package's
  SHA-256 digest to the exact commit and workflow run that produced it. To verify a
  downloaded package with the [GitHub CLI](https://cli.github.com/):

  ```bash
  gh attestation verify Wolfgang.D20.Dice.<version>.nupkg --repo Chris-Wolfgang/D20-Dice
  ```

  A successful verification confirms the package was built by this repo's release workflow.
- **SBOM** — a CycloneDX Software Bill of Materials (`Wolfgang.D20.Dice.bom.json`) is
  generated at release time and attached to the GitHub Release, giving a machine-readable
  inventory of the package's full dependency closure.

> **Not yet author-signed.** The `.nupkg` does not currently carry a NuGet author signature
> (`nuget verify` / trusted-signers), because that requires a code-signing certificate. NuGet
> still applies its own repository signature on publish. Author signing is tracked in
> [#171](https://github.com/Chris-Wolfgang/D20-Dice/issues/171).
