window.BENCHMARK_DATA = {
  "lastUpdate": 1790489268689,
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
      }
    ]
  }
}