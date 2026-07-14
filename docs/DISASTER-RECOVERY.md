# Disaster Recovery — NuGet / GitHub compromise

A runbook for responding to a compromise of the accounts and credentials that
publish and host **D20-Dice**. The goal is to stop the bleeding fast, then
recover cleanly. Work top-to-bottom; the **Immediate** steps are ordered by
priority.

> Related: [`WORKFLOW_SECURITY.md`](WORKFLOW_SECURITY.md) (CI hardening),
> [`RELEASE-WORKFLOW-SETUP.md`](RELEASE-WORKFLOW-SETUP.md) (how releases publish).

## What can be compromised

- **NuGet** — the `NUGET_API_KEY` secret, or the nuget.org account/owner that can
  push and unlist `Wolfgang.D20.Dice`.
- **GitHub** — a maintainer account, a PAT / fine-grained token, an Actions
  secret, a deploy key, or an OAuth/GitHub App with write access.
- **A release** — a malicious or tampered package version published to NuGet.

## Immediate response (first 30 minutes)

1. **Revoke the exposed credential.**
   - Leaked `NUGET_API_KEY`: sign in to nuget.org → *API Keys* → **delete** the
     key. Also delete the `NUGET_API_KEY` repo secret (Settings → Secrets and
     variables → Actions) so no workflow can reuse it.
   - Leaked GitHub PAT / token: GitHub → Settings → Developer settings →
     **revoke** the token. For a fine-grained token, also remove its repo access.
   - Compromised GitHub account: reset the password, sign out all sessions,
     confirm 2FA is on and the recovery methods are yours.
2. **Freeze publishing.** Temporarily disable the `release.yaml` workflow
   (Actions → Release → ⋯ → Disable workflow) so nothing new can ship while you
   investigate.
3. **Assess exposure.** Review the GitHub **audit log** (org/user Settings →
   Logs) and the nuget.org package **version history** for anything you did not
   publish. Note timestamps.

## Contain

4. **Rotate every secret**, not just the leaked one — assume lateral movement:
   `NUGET_API_KEY` and any other Actions/Dependabot secrets. Regenerate the
   NuGet key with the **narrowest** scope (push for this package/glob only).
5. **Review access surface:** deploy keys, webhooks, installed GitHub Apps and
   OAuth apps, org members and their roles, and any forks with unexpected write
   access. Remove anything unrecognized.
6. **Check the code + CI for tampering:** recent commits to `main`, changes to
   `.github/workflows/*`, `Directory.Build.props`, and the branch ruleset
   ("Protect main branch") — confirm required checks and bypass actors are intact.

## Recover a bad package version

NuGet packages **cannot be deleted** (only the owner, and only within a short
window; otherwise they are permanent). To neutralize a malicious version:

7. **Unlist** the bad version on nuget.org (Manage package → *Listing* → uncheck
   the version). Unlisting hides it from search/restore-by-range but leaves it
   installable by exact version, so also:
8. **Deprecate** the bad version (Manage package → *Deprecation*) with reason
   *Other* and a message pointing at the fixed version.
9. **Publish a clean version.** Rebuild from a known-good commit, bump the
   patch/minor `<Version>`, and release through the normal `release.yaml` flow
   (a fresh, scoped `NUGET_API_KEY`). Never try to re-push the same version number.
10. **Notify** if consumers may have pulled the bad version: a GitHub Security
    Advisory on the repo and a note in `CHANGELOG.md`.

## Restore normal operation

11. Re-enable `release.yaml` once secrets are rotated and the tree is verified.
12. Confirm the required-checks ruleset on `main` is active and unchanged.
13. Write a short post-incident note (what happened, blast radius, fixes) — an
    ADR under [`adr/`](adr/) is a good home if the response changed a process.

## Prevention checklist

- 2FA enforced on all maintainer and org accounts.
- Prefer **OIDC trusted publishing** over a long-lived `NUGET_API_KEY` where the
  package supports it (removes the standing secret entirely).
- API keys scoped to the single package/glob, with a short expiry.
- Actions secrets are least-privilege; workflows pinned and reviewed
  (see `WORKFLOW_SECURITY.md`).
- Branch ruleset on `main` requires the full check set before merge.
- Secret scanning + push protection enabled on the repo.
