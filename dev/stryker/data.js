window.BENCHMARK_DATA = {
  "lastUpdate": 1792304057567,
  "repoUrl": "https://github.com/Chris-Wolfgang/D20-Dice",
  "entries": {
    "Mutation score": [
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "2cbc43b4a6d330f404e38a53f0cbde6758c4a5fb",
          "message": "security(ci): silence zizmor self-repository on the docfx call, with the reason (#340)\n\nzizmor flags `uses: ./.github/workflows/docfx.yaml` and asks for GitHub's\ndedicated `$/` self-repository syntax. We cannot switch yet: actionlint is a\nrequired check here and rejects `$/` until rhysd/actionlint#732 ships. The\nalert has therefore been sitting open with nothing to do about it.\n\nrepo-template resolved this by keeping `./` and carrying a rule-specific inline\nignore plus the reason and the exit condition. This takes the same four lines,\nso the alert closes and the next reader learns why the preferred syntax is not\nin use and when to switch.\n\nNo behaviour change: the workflow call is byte-identical apart from the comment.\n\nCo-authored-by: Claude Opus 5 <noreply@anthropic.com>",
          "timestamp": "2026-09-23T12:33:57Z",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/2cbc43b4a6d330f404e38a53f0cbde6758c4a5fb"
        },
        "date": 1790489262948,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 93.24,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "196db8916105bb6731ec7015265d24de71fadacc",
          "message": "test: kill the 10 Stryker survivors in #341 (#345)\n\n- Dice ctor: drop the sideCount < 2 check that duplicated Die's own\n  validation. Die throws the same ArgumentOutOfRangeException with the\n  same parameter name, and dieCount >= 1 guarantees at least one Die is\n  constructed, so the removed statement was an equivalent mutant no test\n  could kill. New tests pin ParamName and the sideCount == 2 boundary.\n- Null-die ArgumentException: assert ParamName and the message.\n- Dice.GetHashCode: pin the exact value for (2, 6, 3) so arithmetic and\n  bitwise mutations of the combination are caught.\n- TryParse: add a 1d2 case (smallest valid side count).\n- Die.GetHashCode: different SideCounts hash differently.\n\nLocal Stryker (net10.0): 142 killed, 0 survived, 100.00 % (was 93.24 %).\n\nCo-authored-by: Claude Opus 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-09-27T22:18:10Z",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/196db8916105bb6731ec7015265d24de71fadacc"
        },
        "date": 1790548067951,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "02fd4a9045f30fb5d14f75efee7808a958d795aa",
          "message": "chore(pack): advance PackageValidation baseline to 0.9.0 (#347)\n\nv0.9.0 is published and indexed on the NuGet flatcontainer (23:22 UTC),\nso the next release is compat-checked against it. No PublicAPI fold\nneeded: PublicAPI.Unshipped.txt has no pending entries.\n\nCo-authored-by: Claude Opus 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-09-27T23:37:18Z",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/02fd4a9045f30fb5d14f75efee7808a958d795aa"
        },
        "date": 1791100125719,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "dependabot[bot]",
            "username": "dependabot[bot]",
            "email": "49699333+dependabot[bot]@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "805aff490384b87f7e4bf80e440253b5612030bb",
          "message": "build(deps): bump urllib3 from 2.7.0 to 2.8.0 in /.github/requirements (#360)\n\nBumps [urllib3](https://github.com/urllib3/urllib3) from 2.7.0 to 2.8.0.\n- [Release notes](https://github.com/urllib3/urllib3/releases)\n- [Changelog](https://github.com/urllib3/urllib3/blob/main/CHANGES.rst)\n- [Commits](https://github.com/urllib3/urllib3/compare/2.7.0...2.8.0)\n\n---\nupdated-dependencies:\n- dependency-name: urllib3\n  dependency-version: 2.8.0\n  dependency-type: indirect\n...\n\nSigned-off-by: dependabot[bot] <support@github.com>\nCo-authored-by: dependabot[bot] <49699333+dependabot[bot]@users.noreply.github.com>\nCo-authored-by: Chris Wolfgang <210299580+Chris-Wolfgang@users.noreply.github.com>",
          "timestamp": "2026-10-10T22:32:16Z",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/805aff490384b87f7e4bf80e440253b5612030bb"
        },
        "date": 1791698828754,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "41a3a8d199c62542745bb22b0740cc91eb0a6c9e",
          "message": "fix(scripts): security-alerts updates the open summary issue instead of filing a new one (#370)\n\nGet-TrackedIssues keyed each marker to the LAST issue in the listing. The REST\nissues listing (state=all) is newest first, so with a closed old summary and an\nopen newer one the map kept the CLOSED issue, and every weekly run filed a\nduplicate summary issue. Prefer the OPEN issue for a marker, and between two of\nthe same state the higher number. Port of Chris-Wolfgang/repo-template#719.\n\nCo-authored-by: Claude Opus 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-10-16T11:50:00Z",
          "url": "https://github.com/Chris-Wolfgang/D20-Dice/commit/41a3a8d199c62542745bb22b0740cc91eb0a6c9e"
        },
        "date": 1792304049685,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      }
    ]
  }
}