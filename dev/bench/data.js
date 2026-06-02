window.BENCHMARK_DATA = {
  "lastUpdate": 1780364387907,
  "repoUrl": "https://github.com/Chris-Wolfgang/D20-Dice",
  "entries": {
    "BenchmarkDotNet": [
      {
        "commit": {
          "author": {
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com",
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang"
          },
          "committer": {
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com",
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang"
          },
          "distinct": true,
          "id": "903ae116472fc24e800500b2214c8aaf8660373e",
          "message": "Protected canonical changes for v0.5.1 (admin-bypass split from PR #144)\n\nExtracted just the protected files from vNext to allow admin-bypass\nmerge of the 'Detect .NET Projects' / 'Protected File Check' guard\npaths separately from the larger v0.5.1 content. After this lands\non main, the vNext PR will only show unprotected drift and can\nmerge without bypass.\n\nProtected files (12):\n- .editorconfig (canonical sync)\n- BannedSymbols.txt (canonical baseline)\n- Directory.Build.props (analyzer set + DBP-Nullable + CI3 metadata)\n- benchmarks/.editorconfig (new — canonical placement for BDN rule carve-outs)\n- tests/.editorconfig (canonical test-rule carve-outs)\n- 7 workflow files (pr.yaml, release.yaml, docfx.yaml, codeql.yaml,\n  build-all-versions.yaml, stryker.yaml, benchmarks.yaml — the last\n  two are new; D8 verify-docs-build, P2 BDN gh-pages chart, etc.)\n\nSame protected-file-pr-split pattern documented in the\nper-repo-release-pilot skill.",
          "timestamp": "2026-06-01T21:38:09-04:00",
          "tree_id": "f4f4d10271dcd9155a371edb276f8da3b2b44335",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/903ae116472fc24e800500b2214c8aaf8660373e"
        },
        "date": 1780364386112,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 6.99952628215154,
            "unit": "ns",
            "range": "± 0.010832077725657994"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 7.005028009414673,
            "unit": "ns",
            "range": "± 0.0029492725530685976"
          }
        ]
      }
    ]
  }
}