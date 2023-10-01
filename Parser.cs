using System;
using System.Collections.Generic;

namespace mpg;

class PatternParser
{
    public PatternParser() {}
    
    public NamedLine ParseNamedLine(string input)
    {
        return new NamedLine();
    }

    public NamedLineOption ParseNamedLineOption(string input)
    {
        return new NamedLineOption(new NamedLine(), 1, 0);
    }

    public List<NamedLineOption> ParseListNamedLineOption(string input)
    {
        return new List<NamedLineOption>();
    }

    public Dictionary<NamedLine, List<NamedLineOption>> ParsePatternDict(string input)
    {
        return new Dictionary<NamedLine, List<NamedLineOption>>();
    }
}