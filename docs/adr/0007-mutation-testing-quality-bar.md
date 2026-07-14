# 7. Mutation-testing quality bar (Stryker)

## Status

Accepted

## Context

Line/branch coverage confirms code is *executed* by tests, not that the tests
would *catch a regression*. Mutation testing (Stryker.NET) measures the latter by
introducing faults and checking whether a test fails. A Stryker workflow already
existed but its break threshold was `0`, so it never gated on the score.

## Decision

Set the Stryker break threshold to **90%**: a change that drops the mutation
score below 90% fails the run (issue #168). The run stays on `workflow_dispatch`
+ a weekly schedule rather than per-PR, because mutation runs are slow.

## Consequences

- Mutation score is a real quality bar, not just a report.
- The 90% threshold has modest headroom over the measured score (~91%); if a
  future run grazes the line the response is to kill surviving mutants (add
  tests) or raise Stryker's per-mutant timeout, not to lower the bar silently.
- Because the run is not per-PR, a score regression is detected on the next
  scheduled/manual run rather than at merge time.
