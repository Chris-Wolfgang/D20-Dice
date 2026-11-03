type: internal

`Dice.TryParse` tokenises with a source-generated regex (`[GeneratedRegex]`) on the net8.0 and net10.0 targets, so parsing runs at full speed under Native AOT instead of silently falling back to the regex interpreter; net462 and netstandard2.0 keep the compiled `Regex` instance.
