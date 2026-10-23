using System.Globalization;
using Wolfgang.D20;

namespace Example1_Console;


internal static class Program
{
    private static int Main()
    {
        Console.Write("Enter dice notation (e.g. 2d6+1d4+3), or press Enter to build the dice step by step: ");
        var notation = ReadLine();

        Dice dice;
        if (notation.Length > 0)
        {
            // TryParse never throws: a failed Result carries the reason, which is what to show the user.
            var parsed = Dice.TryParse(notation);
            if (parsed.Failed)
            {
                Console.WriteLine($"Could not parse \"{notation}\": {parsed.ErrorMessage}");
                return 1;
            }
            dice = parsed.Value!;
        }
        else
        {
            var sideCount = ReadInt32("Enter the number of sides (default 6): ", defaultValue: 6);
            var dieCount = ReadInt32("Enter the number of dice (default 1): ", defaultValue: 1);
            var modifier = ReadInt32("Enter a modifier (default 0): ", defaultValue: 0);
            dice = new Dice(dieCount, sideCount, modifier);
        }

        // Dice.ToString() renders standard notation ("2d6+1d4+3"), always with invariant-culture
        // digits and sign, so there is nothing to hand-format here.
        Console.WriteLine($"Rolling {dice} (range {dice.MinValue} to {dice.MaxValue}):");
        Console.WriteLine($"Result: {dice.Roll()}");
        return 0;
    }



    /// <summary>
    /// Reads a line, treating end-of-input (redirected or closed stdin, where ReadLine returns null) as
    /// an empty line rather than a crash.
    /// </summary>
    private static string ReadLine()
    {
        return (Console.ReadLine() ?? string.Empty).Trim();
    }



    /// <summary>
    /// Prompts for a whole number; an empty answer yields <paramref name="defaultValue"/>, as the prompt
    /// promises. Digits are parsed with the invariant culture, matching the notation the library emits.
    /// </summary>
    private static int ReadInt32(string prompt, int defaultValue)
    {
        Console.Write(prompt);
        var input = ReadLine();
        return input.Length == 0 ? defaultValue : int.Parse(input, CultureInfo.InvariantCulture);
    }
}
