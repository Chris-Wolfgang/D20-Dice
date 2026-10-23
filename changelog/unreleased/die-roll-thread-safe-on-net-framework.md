type: fix

`Die.Roll()` and `Dice.Roll()` are now thread-safe on .NET Framework and .NET Standard 2.0: the shared `System.Random` those targets use is serialised with a lock, so concurrent rolls can no longer corrupt it and leave every later roll stuck on `MinValue`. (.NET 6+ already rolled through the thread-safe `Random.Shared`.)
