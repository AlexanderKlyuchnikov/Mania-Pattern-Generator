using System;
using System.Collections.Generic;

namespace mpg;

class PatternParser
{
    public PatternParser() {}

    public NamedLine ParseNamedLine(string input)
    {
        NamedLineTypes type;
        string value;
        string index;

        string[] inputarr = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (inputarr.Length == 1)
            index = "";
        else
            index = inputarr[1];

        if (inputarr[0][0] == '"')
        {
            type = NamedLineTypes.String;
            value = inputarr[0][1..^1];
        }
        else
        {
            type = NamedLineTypes.NoteCount;
            value = inputarr[0];
        }
        return new NamedLine(type, value, index);
    }

    public NamedLineOption ParseNamedLineOption(string input)
    {
        int delpos = input.IndexOfAny(new char[]{'(', '[', '{'});
        string nmln = input[..delpos];
        NamedLine line = this.ParseNamedLine(nmln);
        int posbeg = input.IndexOf('(') + 1;
        int posend = input.IndexOf(')');
        int value = int.Parse(input.Substring(posbeg, posend - posbeg));
        PlacementWay way = PlacementWay.Random;
        posbeg = input.IndexOf('[') + 1;
        posend = input.IndexOf(']');
        string waystr = input.Substring(posbeg, posend - posbeg);
        switch (waystr)
        {
            case "StreamStrong":
                way = PlacementWay.StreamStrong;
                break;
            case "StreamWeak":
                way = PlacementWay.StreamWeak;
                break;
            case "Random":
                way = PlacementWay.Random;
                break;
        }
        return new NamedLineOption(line, value, way);
    }

    public List<NamedLineOption> ParseListNamedLineOption(string input)
    {
        List<NamedLineOption> result = new List<NamedLineOption>();
        foreach (string item in input.Split(','))
        {
            result.Add(this.ParseNamedLineOption(item));
        }
        return result;
    }

    public Dictionary<NamedLine, List<NamedLineOption>> ParsePatternDict(string[] input)
    {
        var result = new Dictionary<NamedLine, List<NamedLineOption>>();
        int pos;
        string keystr;
        string valuestr;
        //input.IndexOf('=');
        for (int i = 0; i < input.Length; i++)
        {
            pos = input[i].IndexOf('=');
            keystr = input[i][..pos];
            pos += 2;
            valuestr = input[i][pos..];
            result.Add(ParseNamedLine(keystr), ParseListNamedLineOption(valuestr));
        }
        return result;
    }
}