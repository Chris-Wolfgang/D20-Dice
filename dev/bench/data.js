window.BENCHMARK_DATA = {
  "lastUpdate": 1783962752565,
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
          "id": "9af020d4ba1c97bbf0a9134145958cbb218038a8",
          "message": "Merge pull request #197 from Chris-Wolfgang/dependabot/github_actions/github-actions-39b8605068\n\nBump the github-actions group with 2 updates",
          "timestamp": "2026-06-27T13:59:13-04:00",
          "tree_id": "319d64e59248c2cbc7ba2fe215d62cdf0a110cff",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/9af020d4ba1c97bbf0a9134145958cbb218038a8"
        },
        "date": 1782583219382,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 33.364256183306374,
            "unit": "ns",
            "range": "± 0.14051964120484764"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 27.184293270111084,
            "unit": "ns",
            "range": "± 0.593518597173771"
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
          "id": "ee0df7895ee8e016b57c2b43e130ceacdd0bda77",
          "message": "Merge pull request #206 from Chris-Wolfgang/dependabot/nuget/dotnet-dependencies-7d6ae261de\n\nBump the dotnet-dependencies group with 5 updates",
          "timestamp": "2026-07-09T21:22:08-04:00",
          "tree_id": "5bdf2aa3795461a28b523ad72c99b653f79f5ab9",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/ee0df7895ee8e016b57c2b43e130ceacdd0bda77"
        },
        "date": 1783646594792,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 29.87845532099406,
            "unit": "ns",
            "range": "± 0.10253218446654327"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 28.632192850112915,
            "unit": "ns",
            "range": "± 0.3013453169862871"
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
          "id": "2beb2c62ab6451b611de9abb3f72ee7079a1f835",
          "message": "Merge pull request #207 from Chris-Wolfgang/chore/release-v0.6.1\n\nchore: release v0.6.1",
          "timestamp": "2026-07-11T20:11:15-04:00",
          "tree_id": "6ba964c076a8b14ce5a2caea67febebf914c4268",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/2beb2c62ab6451b611de9abb3f72ee7079a1f835"
        },
        "date": 1783815147491,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 26.990720609823864,
            "unit": "ns",
            "range": "± 0.35232385832557744"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 26.95057554046313,
            "unit": "ns",
            "range": "± 0.08726108106909931"
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
          "id": "8ba729c31a0bc91b52db2cdb1c2c3a72a2e1102b",
          "message": "Merge pull request #208 from Chris-Wolfgang/feature/49-average-roll\n\nAdd AverageRoundedUp/AverageRoundedDown extensions (#49)",
          "timestamp": "2026-07-12T14:54:52-04:00",
          "tree_id": "cb7b49606bc171483a1bfc6751eedf04292e0355",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/8ba729c31a0bc91b52db2cdb1c2c3a72a2e1102b"
        },
        "date": 1783882558008,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 29.320256461699802,
            "unit": "ns",
            "range": "± 0.05693735488579219"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 27.24166006843249,
            "unit": "ns",
            "range": "± 0.09815517401402135"
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
          "id": "ef26419ce120da8c3754df24ee83bdbaca606d68",
          "message": "Merge pull request #209 from Chris-Wolfgang/feature/177-globalization\n\nFormat dice notation with invariant culture (#177)",
          "timestamp": "2026-07-12T21:21:55-04:00",
          "tree_id": "1332cbb0c969aa8b5bf47bcf1afea14bf790ffc3",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/ef26419ce120da8c3754df24ee83bdbaca606d68"
        },
        "date": 1783905772734,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 28.417409479618073,
            "unit": "ns",
            "range": "± 0.5185851983574676"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 28.1616437236468,
            "unit": "ns",
            "range": "± 0.49632831517721787"
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
          "id": "9fbd0020589a95bba0118ce00aa82a11ed932b6c",
          "message": "Merge pull request #210 from Chris-Wolfgang/feature/179-allocation-free\n\nMake Dice.Roll allocation-free on the hot path (#179)",
          "timestamp": "2026-07-12T21:40:57-04:00",
          "tree_id": "d7be88473e587e6dd21a93bfb5caf78a1458c827",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/9fbd0020589a95bba0118ce00aa82a11ed932b6c"
        },
        "date": 1783906922203,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 7.819701408346494,
            "unit": "ns",
            "range": "± 0.02286199451913676"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 7.443056474129359,
            "unit": "ns",
            "range": "± 0.012417709877980403"
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
          "id": "82a3639cb5e4624d5799c838374cd50c5d40a2f5",
          "message": "Merge pull request #212 from Chris-Wolfgang/feature/169-apicompat\n\nEnable SDK PackageValidation for API/ABI compat (#169)",
          "timestamp": "2026-07-13T13:10:59-04:00",
          "tree_id": "2f39b5beddaa5d30f90b07ae6acfad8933d40ac0",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/82a3639cb5e4624d5799c838374cd50c5d40a2f5"
        },
        "date": 1783962750850,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD20",
            "value": 7.558847948908806,
            "unit": "ns",
            "range": "± 0.2476727583143182"
          },
          {
            "name": "Wolfgang.D20.Benchmarks.DiceBenchmarks.RollD6",
            "value": 7.815252934892972,
            "unit": "ns",
            "range": "± 0.0044137984963309555"
          }
        ]
      }
    ]
  }
}