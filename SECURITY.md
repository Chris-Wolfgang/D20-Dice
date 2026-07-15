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

- **Release path**: NuGet **API key**. `.github/workflows/release.yaml` (triggered on `release: published`) pushes with the long-lived `NUGET_API_KEY` GitHub Actions secret. That secret **is** the standing release credential — during an incident, delete the `NUGET_API_KEY` repo secret and revoke/rotate the corresponding key on nuget.org, then check the NuGet account for any other long-lived API keys and delete anything you don't recognize.
- **Fallback**: none. If the release credential is compromised, the incident is at the GitHub-account / NuGet-account level.
- **Hardening direction**: migrating to OIDC / NuGet Trusted Publishing (via `NuGet/login@v1`, as piloted on ETL-SqlBulkCopy) would remove this standing secret entirely.
- **Owner**: @Chris-Wolfgang.
- **Downstream consumers**: none known inside the Wolfgang.* org at time of writing; the package is public on nuget.org, so unknown downstream consumers may exist. Re-check via `dotnet-outdated`, GitHub code-search, and the NuGet package's `Used By` dependents list before communicating during an incident.
- **Package coordinates for unlisting**: `Wolfgang.D20.Dice` on nuget.org — <https://www.nuget.org/packages/Wolfgang.D20.Dice/>.
