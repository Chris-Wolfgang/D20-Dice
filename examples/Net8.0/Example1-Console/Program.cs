using Wolfgang.D20;

namespace Example1_Console;


internal static class Program
{
    private static void Main()
    {

        Console.Write("Enter the number of sides: ");
        var sideCount = int.Parse(Console.ReadLine()!);

        Console.Write("Enter the number of dice: ");
        var dieCount = int.Parse(Console.ReadLine()!);

        Console.Write("Enter a modifier (default 0): ");
        var modifier = int.Parse(Console.ReadLine()!);


        var dice = new Dice(dieCount, sideCount, modifier);

        var modifierString = modifier switch
        {
            0 => string.Empty,
            > 0 => $"+{modifier}",
            < 0 => modifier.ToString()
        };

        Console.WriteLine($"Rolling {dieCount}d{sideCount}{modifierString}:");
        Console.WriteLine($"Result: {dice.Roll()}");
    }
}
