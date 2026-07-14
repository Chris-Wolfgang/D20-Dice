# 5. Verify AOT compatibility via analyzers

## Status

Accepted

## Context

Consumers may publish with Native AOT or trimming. A library cannot be
AOT-published on its own, so it needs a way to guarantee it stays AOT/trim-safe.
There are two complementary levers: the SDK's static analyzers, and an actual
publish-and-run of a consumer.

## Decision

Enable `IsAotCompatible=true` on the net8.0/net10.0 builds of the library. This
turns on the trim, AOT, and single-file analyzers, so an AOT-incompatible change
surfaces as an `IL2xxx`/`IL3xxx` warning and fails the build under
`TreatWarningsAsErrors` (issue #175). As a runtime complement, a small
`PublishAot` smoke consumer is published and executed in CI (issue #220).

## Consequences

- AOT/trim regressions are caught at build time on the modern TFMs, and at
  publish/run time in the AOT smoke workflow.
- `IsAotCompatible` is only valid on net8.0+, so it is scoped away from
  `net462` / `netstandard2.0`.
- `RegexOptions.Compiled` is not flagged by the analyzers; it simply falls back
  to interpreted under AOT at runtime, which is acceptable.
