# d20 Dice

## Description

Represents a simple dice rolling library for D20 systems, 
allowing you to roll dice using either individual values 
or dice notation (e.g., `XdY+Z`).



## Usage

### Using individual values
```csharp
using Wolfgang.D20;

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


        var dice = new Dice(dieCount, sideCount, modifier);

        Console.WriteLine($"Rolling {dieCount}d{sideCount}{(modifier != 0 ? (modifier > 0 ? "+" : "") + modifier : "")}:");
        Console.WriteLine($"Result: {dice.Roll()}");
    }
}
```

### Using Dice Notation
```csharp
using Wolfgang.D20;

internal class Program
{
    private static void Main(string[] args)
    {

        Console.Write("Create dice using using dice notation (XdY+Z):");
        var notation = Console.ReadLine()

        var dice = new Dice(notation);
    }
}
```
