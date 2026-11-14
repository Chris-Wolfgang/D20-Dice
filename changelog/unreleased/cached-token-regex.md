type: internal

The dice-notation parser uses one cached `Regex` on every target instead of the regex source generator on net8.0 and net10.0, so the library builds byte-identically on Linux and Windows (#378). Parsing results are unchanged.
