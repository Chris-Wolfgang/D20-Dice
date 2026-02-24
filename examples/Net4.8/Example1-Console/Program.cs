using System;
using Wolfgang.D20;

namespace Example1_Console
{
    internal static class Program
    {
        private static void Main()
        {

            Console.Write("Enter the number of sides: ");
            var sideCount = int.Parse(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

            Console.Write("Enter the number of dice: ");
            var dieCount = int.Parse(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

            Console.Write("Enter a modifier (default 0): ");
            var modifier = int.Parse(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

            var dice = new Dice(dieCount, sideCount, modifier);

            string modifierString;
            if (modifier > 0)
            {
                modifierString = $"+{modifier}";
            }
            else if (modifier < 0)
            {
                modifierString = modifier.ToString();
            }
            else
            {
                modifierString = "";
            }

            Console.WriteLine($"Rolling {dieCount}d{sideCount}{modifierString}:");
            Console.WriteLine($"Result: {dice.Roll()}");
        }
    }
}
