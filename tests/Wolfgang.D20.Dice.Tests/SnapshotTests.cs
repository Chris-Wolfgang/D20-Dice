#if NET10_0_OR_GREATER
using System.Text;
using Xunit;
using static VerifyXunit.Verifier;

namespace Wolfgang.D20.Tests.Unit;

/// <summary>
/// Snapshot / approval tests (Verify) that pin the rendered dice-notation format as a golden master.
/// A single verified snapshot captures many representative configurations at once, so a change to the
/// <c>ToString</c> format surfaces every affected case in one diff. Guarded to net10.0 so a single
/// verified snapshot is produced (the format is TFM-independent, and net10.0 runs in CI); the exact
/// per-case assertions live in the other test files.
/// </summary>
public class SnapshotTests
{

    [Fact]
    public Task Dice_notation_golden_master()
    {
        var builder = new StringBuilder();

        void Add(string label, string notation) => builder.AppendLine($"{label,-24} => {notation}");

        Add("Die(6)", new Die(6).ToString());
        Add("Die(20)", new Die(20).ToString());
        Add("Dice(2,6)", new Dice(2, 6).ToString());
        Add("Dice(2,6,+3)", new Dice(2, 6, 3).ToString());
        Add("Dice(2,6,-1)", new Dice(2, 6, -1).ToString());
        Add("Dice(1,20)", new Dice(1, 20).ToString());
        Add("Dice([d6,d6,d4])", new Dice(new[] { new Die(6), new Die(6), new Die(4) }).ToString());
        Add("Dice([d8,d4],+2)", new Dice(new[] { new Die(8), new Die(4) }, 2).ToString());
        Add("Dice([d10,d10,d6],-2)", new Dice(new[] { new Die(10), new Die(10), new Die(6) }, -2).ToString());

        // Keep verified snapshots together under a dedicated Snapshots/ folder.
        return Verify(builder.ToString()).UseDirectory("Snapshots");
    }

}
#endif
