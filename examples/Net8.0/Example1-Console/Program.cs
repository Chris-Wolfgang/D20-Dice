namespace Example1_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.Write("Enter the number of sides: ");
            var sideCount = int.Parse( Console.ReadLine());

            Console.Write("Enter the number of dice: ");
            var dieCount = int.Parse(Console.ReadLine());

            Console.Write("Enter a modifier (default 0): ");
            var modifier = int.Parse(Console.ReadLine());


            var dice = new Wolfgang.DnD.Dice(dieCount, sideCount, modifier);

            Console.WriteLine($"Rolling {dieCount}d{sideCount}{(modifier != 0 ? (modifier > 0 ? "+" : "") + modifier : "")}:");
            Console.WriteLine($"Result: {dice.Roll()}");
        }
    }
}