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
        if (inputarr.Length == 0)
            throw new ParseException("Empty NamedLine string");
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
        int value = -1;
        if (!int.TryParse(input[posbeg..posend], out value))
            throw new ParseException("Not int option value");
            
        PlacementWay way = PlacementWay.Random;
        posbeg = input.IndexOf('[') + 1;
        posend = input.IndexOf(']');
        string waystr = input[posbeg..posend];
        way = waystr switch
        {
            "StreamStrong" => PlacementWay.StreamStrong,
            "StreamWeak" => PlacementWay.StreamWeak,
            "Random" => PlacementWay.Random,
            _ => throw new ParseException("Unknown option placement way"),
        };
        return new NamedLineOption(line, value, way);
    }

    public NamedLineSetup ParseNamedLineSetup(string input)
    {
        int pos = input.IndexOf('[') + 1;
        int pos2 = input.IndexOf(']');
        PlacementWay way = 0;
        string waystr = input[pos..pos2];
        way = waystr switch
        {
            "StreamStrong" => PlacementWay.StreamStrong,
            "StreamWeak" => PlacementWay.StreamWeak,
            "Random" => PlacementWay.Random,
            _ => throw new ParseException("Unknown setup placement way"),
        };
        return new NamedLineSetup(ParseNamedLine(input[..(pos-1)]), way);
    }

    public List<NamedLineOption> ParseListNamedLineOption(string input)
    {
        List<NamedLineOption> result = new List<NamedLineOption>();
        string[] arr = input.Split(',');
        for (int i = 0; i < arr.Length; i++)
        {
            try
            {
                result.Add(this.ParseNamedLineOption(arr[i]));
            }
            catch (ParseException e)
            {
                string message = "Invalid " + (i + 1).ToString() + " option: " + e.Message;
                throw new ParseException(message);
            }
        }

        return result;
    }

    public Dictionary<NamedLine, List<NamedLineOption>> ParsePatternDict(string[] input)
    {
        var result = new Dictionary<NamedLine, List<NamedLineOption>>();
        int pos;
        string keystr;
        string valuestr;
        for (int i = 0; i < input.Length; i++)
        {
            pos = input[i].IndexOf('=');
            keystr = input[i][..pos];
            pos += 2;
            valuestr = input[i][pos..];
            try
            {
                result.Add(ParseNamedLine(keystr), ParseListNamedLineOption(valuestr));
            }
            catch (ParseException e)
            {
                string message = "Invalid " + (i + 1).ToString() + " transition: " + e.Message;
                throw new ParseException(message);
            }
        }
        return result;
    }

    public Pattern ParsePattern(string[] input)
    {
        string str;
        int pos;
        var init = new NamedLine();
        var defline = new NamedLineSetup();
        var linedict = new Dictionary<NamedLine, List<NamedLineOption>>();
        try
        {
            for (int i = 0; i < input.Length; i++)
            {
                str = input[i];
                if (string.Equals(str, "start"))
                {
                    linedict = ParsePatternDict(input[(i+1)..]);
                    break;
                }
                pos = str.IndexOf('=');
                if (pos == -1)
                {
                    string message = "Not found \"=\" in " + (i + 1).ToString() + " line";
                    throw new ParseException(message);
                }
                switch (input[i][..pos])
                {
                    case "init":
                        init = ParseNamedLine(input[i][(pos+1)..]);
                        break;
                    case "default":
                        defline = ParseNamedLineSetup(input[i][(pos+1)..]);
                        break;
                    default:
                        string message = "Unknown key in" + (i + 1).ToString() + "operator";
                        throw new ParseException(message);
                }
            }
            if (linedict.Count == 0)
                throw new ParseException("Not found start label");
        }
        catch (ParseException e)
        {
            string message = "Parse error: " + e.Message;
            throw new ParseException(message);
        }
        return new Pattern(init, defline, linedict);
    }
}