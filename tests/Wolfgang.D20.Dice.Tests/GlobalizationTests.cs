using System.Globalization;
using Xunit;

namespace Wolfgang.D20.Tests.Unit;

public class GlobalizationTests
{

    /// <summary>
    /// Runs <paramref name="action"/> with both <see cref="Thread.CurrentCulture"/> and
    /// <see cref="Thread.CurrentUICulture"/> set to <paramref name="culture"/>, restoring both afterwards.
    /// </summary>
    private static void WithCulture(CultureInfo culture, Action action)
    {
        var thread = Thread.CurrentThread;
        var originalCulture = thread.CurrentCulture;
        var originalUICulture = thread.CurrentUICulture;
        thread.CurrentCulture = culture;
        thread.CurrentUICulture = culture;
        try
        {
            action();
        }
        finally
        {
            thread.CurrentCulture = originalCulture;
            thread.CurrentUICulture = originalUICulture;
        }
    }



    // Cultures whose number formatting differs from the invariant culture in ways that could leak into
    // dice notation (native digits, non-ASCII signs, RTL scripts).
    public static IEnumerable<object[]> Cultures()
    {
        // Cultures called out in the #177 acceptance criteria.
        yield return new object[] { "en-US" };
        yield return new object[] { "tr-TR" };
        yield return new object[] { "zh-CN" };
        yield return new object[] { "ja-JP" };
        // Additional cultures whose number formatting stresses digits / signs / RTL scripts.
        yield return new object[] { "ar-SA" };
        yield return new object[] { "fa-IR" };
        yield return new object[] { "fi-FI" };
        yield return new object[] { "de-DE" };
        yield return new object[] { "th-TH" };
        yield return new object[] { "hi-IN" };
    }



    [Theory]
    [MemberData(nameof(Cultures))]
    public void Die_ToString_is_culture_invariant(string cultureName)
    {
        WithCulture(new CultureInfo(cultureName), () =>
        {
            // Arrange & Act
            var actual = new Die(20).ToString();

            // Assert
            Assert.Equal("d20", actual);
        });
    }



    [Theory]
    [MemberData(nameof(Cultures))]
    public void Dice_ToString_is_culture_invariant(string cultureName)
    {
        WithCulture(new CultureInfo(cultureName), () =>
        {
            // Arrange & Act - includes a negative modifier, the culture-sensitive case.
            var actual = new Dice(2, 6, -3).ToString();

            // Assert
            Assert.Equal("2d6-3", actual);
        });
    }



    [Theory]
    [MemberData(nameof(Cultures))]
    public void Dice_ToString_round_trips_through_TryParse_under_any_culture(string cultureName)
    {
        WithCulture(new CultureInfo(cultureName), () =>
        {
            // Arrange
            var original = new Dice(2, 6, -3);

            // Act
            var parsed = Dice.TryParse(original.ToString());

            // Assert
            Assert.True(parsed.Succeeded, parsed.ErrorMessage);
            Assert.Equal(original, parsed.Value);
        });
    }



    [Fact]
    public void Dice_ToString_ignores_a_hostile_current_culture_negative_sign()
    {
        // Arrange - a culture whose NegativeSign is a non-ASCII sentinel. int.ToString always emits ASCII
        // digits, so the negative sign is the axis that a current-culture leak would corrupt.
        var hostile = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        hostile.NumberFormat.NegativeSign = "−"; // MINUS SIGN, not ASCII '-'

        WithCulture(hostile, () =>
        {
            // Act
            var actual = new Dice(1, 6, -1).ToString();

            // Assert - the fix formats invariantly, so the ASCII '-' is used regardless of the culture.
            Assert.Equal("1d6-1", actual);
        });
    }

}
