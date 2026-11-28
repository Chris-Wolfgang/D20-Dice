// Native-AOT smoke consumer for Wolfgang.D20.Dice (#220).
//
// Exercises the public surface after native-AOT publish and returns a non-zero
// exit code if any invariant is violated, so the CI job fails if the library
// stops being AOT-safe at publish/run time. Kept deliberately simple — this is a
// smoke test, not a behavioural test suite (those live in the unit-test project).

using Wolfgang.D20;

static int Fail(string message)
{
    Console.Error.WriteLine($"AOT smoke FAILED: {message}");
    return 1;
}

// Dice: construct, roll within bounds, ToString round-trips through TryParse.
var dice = new Dice(2, 6, 1);

var roll = dice.Roll();
if (roll < dice.MinValue || roll > dice.MaxValue)
{
    return Fail($"Dice.Roll() {roll} outside [{dice.MinValue}, {dice.MaxValue}]");
}

var notation = dice.ToString();
var parsed = Dice.TryParse(notation);
if (parsed.Failed || !dice.Equals(parsed.Value))
{
    return Fail($"Dice.TryParse round-trip broke for \"{notation}\"");
}

// AverageRounded* extensions (the #49 surface).
var down = dice.AverageRoundedDown();
var up = dice.AverageRoundedUp();
if (down > up)
{
    return Fail($"AverageRoundedDown ({down}) > AverageRoundedUp ({up})");
}

// Die: construct, roll within bounds, notation.
var die = new Die(20);
var dieRoll = die.Roll();
if (dieRoll < die.MinValue || dieRoll > die.MaxValue)
{
    return Fail($"Die.Roll() {dieRoll} outside [{die.MinValue}, {die.MaxValue}]");
}
if (!string.Equals(die.ToString(), "d20", StringComparison.Ordinal))
{
    return Fail($"Die.ToString() was \"{die}\", expected \"d20\"");
}

Console.WriteLine($"AOT smoke OK: {dice} rolled {roll} (avg {down}-{up}); {die} rolled {dieRoll}");
return 0;
