window.BENCHMARK_DATA = {
  "lastUpdate": 1781904528007,
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
      },
      {
        "commit": {
          "author": {
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com",
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang"
          },
          "committer": {
            "email": "noreply@github.com",
            "name": "GitHub",
            "username": "web-flow"
          },
          "distinct": true,
          "id": "5ba9158c444da778971dc0a2a7cf4a8fb032df49",
          "message": "Merge pull request #144 from Chris-Wolfgang/vNext\n\nRelease v0.5.1: canonical maintenance round + AssemblyVersion fix",
          "timestamp": "2026-06-01T21:47:34-04:00",
          "tree_id": "ef4e653c7945b518ebde9da9570df79e405aa021",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/5ba9158c444da778971dc0a2a7cf4a8fb032df49"
        },
        "date": 1780364921030,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 6.8286619037389755,
            "unit": "ns",
            "range": "± 0.1988365623448172"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 6.814373339215915,
            "unit": "ns",
            "range": "± 0.0498974994690444"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com",
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang"
          },
          "committer": {
            "email": "noreply@github.com",
            "name": "GitHub",
            "username": "web-flow"
          },
          "distinct": true,
          "id": "bfaeba636a941dc13e754b73089e615235e54470",
          "message": "Merge pull request #153 from Chris-Wolfgang/fix/obj-is-null-consistency\n\nfix: 'obj == null' → 'obj is null' for consistency (orphan rescue)",
          "timestamp": "2026-06-01T22:17:36-04:00",
          "tree_id": "985b282eb67f18c0708c1ebb6285770c5638bd76",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/bfaeba636a941dc13e754b73089e615235e54470"
        },
        "date": 1780366716258,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 7.208928768833478,
            "unit": "ns",
            "range": "± 0.049922945805450175"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 7.271819959084193,
            "unit": "ns",
            "range": "± 0.008257389901619179"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com",
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang"
          },
          "committer": {
            "email": "noreply@github.com",
            "name": "GitHub",
            "username": "web-flow"
          },
          "distinct": true,
          "id": "58bb569ebde2c36bfd7d0469924260473d1944d9",
          "message": "Merge pull request #195 from Chris-Wolfgang/feature/48-dice-collection\n\nMake Dice a collection of Die for heterogeneous rolls (#48)",
          "timestamp": "2026-06-19T17:27:43-04:00",
          "tree_id": "f5ed58bedfd420542f0a5b54440a6aff1a7085c0",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/58bb569ebde2c36bfd7d0469924260473d1944d9"
        },
        "date": 1781904526351,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 27.825825442870457,
            "unit": "ns",
            "range": "± 0.13675570085161365"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 26.603730867306393,
            "unit": "ns",
            "range": "± 0.12859754339945964"
          }
        ]
      }
    ]
  }
}