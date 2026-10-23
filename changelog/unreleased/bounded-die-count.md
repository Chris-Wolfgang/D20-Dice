type: breaking

A `Dice` pool now holds at most `Dice.MaxDieCount` (10,000) dice: the constructors throw and `Dice.TryParse` fails with `"Die count must be at most 10000."` above that, counting across terms, so untrusted notation such as `"2000000000d6"` can no longer exhaust memory or stall the process. Pools above 10,000 dice were previously accepted (ADR-0009).
