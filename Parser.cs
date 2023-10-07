using System;
using System.Collections.Generic;
using System.Linq;

namespace mpg;

class PatternParser
{
    public NamedLine initline = new();
    public APlacementWay defaultPlacementWay = new StreamWay();
    public PatternParser() {}

    public string CropSpaces(string str)
    {
        int begpos = -1;
        int endpos = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (!((str[i] == ' ') || (((byte)str[i]) == 13) || (((byte)str[i]) == 10)))
            {
                begpos = i;
                break;
            }
        }
        if (begpos == -1)
            return "";
        for (int i = str.Length - 1; i > -1; i--)
        {
            if (!((str[i] == ' ') || (((byte)str[i]) == 13) || (((byte)str[i]) == 10)))
            {
                endpos = i;
                break;
            }
        }
        return str[begpos..(endpos+1)];
    }
    
    public string[] SplitAttributes(string str)
    {
        List<string> result = new();
        int count1 = 0, count2 = 0, count3 = 0;
        int posbeg = -1, posend = -1;
        for (int i = 0; i < str.Length; i++)
            switch (str[i]) {
                case '(':
                    count1++;
                    break;
                case ')':
                    count1--;
                    break;
                case '[':
                    count2++;
                    break;
                case ']':
                    count2--;
                    break;
                case '{':
                    count3++;
                    break;
                case '}':
                    count1--;
                    break;
                case ',':
                    if ((count1 == 0) && (count2 == 0) && (count3 == 0))
                    {
                        posbeg = posend + 1;
                        posend = i;
                        result.Add(str[posbeg..posend]);
                    }
                    break;
                default:
                    break;
        }
        result.Add(str[(posend + 1)..]);
        return result.ToArray();
    }

    public NamedLine ParseNamedLine(string input)
    {
        if (input[0] != '[')
            throw new ParseException("First character in NamedLine definition is not \"[\"");
        if (input.Last() != ']')
            throw new ParseException("Last character in NamedLine definition is not \"]\"");
        
        string[] args = this.SplitAttributes(input[1..(input.Length - 1)]);
        if (args.Length != 2)
            throw new ParseException("Wrong number of NamedLine arguments: " + args.Length.ToString() + " instead of 2");

        NamedLineTypes type;
        string value = this.CropSpaces(args[0]);
        if (value.Length == 0)
            throw new ParseException("NamedLine value is empty");
        if ((value[0] == '\"') && (value.Last() == '\"') && (value.Length != 1))
        {
            type = NamedLineTypes.String;
            value = value[1..(value.Length - 1)];
        }
        else
        {
            type = NamedLineTypes.NoteCount;
        }
         
        string index = this.CropSpaces(args[1]);
        return new NamedLine(type, value, index);
    }

    public int ParseValue(string input)
    {
        if (input.Length == 0)
            return 1;
        if (!int.TryParse(input, out int result))
            throw new ParseException("Not int option value: " + input);
        if (result <= 0)
            throw new ParseException("Not positive option value: " + result.ToString());
        return result;
    }

    public APlacementWay ParsePlacementWay(string input)
    {
        if (input.Length == 0)
            return this.defaultPlacementWay;
        return input switch
        {
            "StreamStrong" => new StreamWay(WayType.Strong, 2, 0),
            "StreamWeak" => new StreamWay(WayType.Weak, 1, 0),
            //"Random" => PlacementWay.Random,
            _ => throw new ParseException("Unknown option placement way: " + input),
        }; 
    }

    public NamedLineOption ParseNamedLineOption(string input)
    {
        if (input[0] != '(')
            throw new ParseException("First character in NamedLine option is not \"(\"");
        if (input.Last() != ')')
            throw new ParseException("Last character in NamedLine option is not \")\"");
        
        string[] args = this.SplitAttributes(input[1..(input.Length - 1)]);
        if (args.Length != 3)
            throw new ParseException("Wrong number of option arguments: " + args.Length.ToString() + " instead of 3");

        NamedLine line = this.ParseNamedLine(this.CropSpaces(args[0]));
        int value = this.ParseValue(this.CropSpaces(args[1]));
        APlacementWay way = this.ParsePlacementWay(this.CropSpaces(args[2]));

        return new NamedLineOption(line, value, way);
    }

    public List<NamedLineOption> ParseListNamedLineOption(string input)
    {
        List<NamedLineOption> result = new();
        string[] arr = this.SplitAttributes(input);
        if (arr.Length == 0)
            throw new ParseException("Empty list of next line options");
        for (int i = 0; i < arr.Length; i++)
        {
            try
            {
                result.Add(this.ParseNamedLineOption(this.CropSpaces(arr[i])));
            }
            catch (ParseException e)
            {
                string message = "Invalid " + (i + 1).ToString() + " option: " + e.Message;
                throw new ParseException(message);
            }
        }

        return result;
    }

    public void ParseSettings(string input)
    {
        int pos;
        string str;
        string[] trans = input.Split(';', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < trans.Length; i++)
        {
            pos = trans[i].IndexOf('=');
            if (pos == -1)
                continue;
            str = this.CropSpaces(trans[i][0..pos]);
            switch (str)
            {
                case "init":
                    this.initline = this.ParseNamedLine(this.CropSpaces(trans[i][(pos+1)..]));
                    break;
                case "default_placement_way":
                    this.defaultPlacementWay = this.ParsePlacementWay(this.CropSpaces(trans[i][(pos+1)..]));
                    break;
                default:
                    throw new ParseException("Unknown setting: " + str);
            }
        }
    }

    public Dictionary<NamedLine, List<NamedLineOption>> ParsePatternDict(string input)
    {
        var result = new Dictionary<NamedLine, List<NamedLineOption>>();
        int pos;
        string keystr;
        string valuestr;

        string[] trans = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < trans.Length; i++)
        {
            pos = trans[i].IndexOf('=');
            if (pos == -1)
                continue;
            keystr = this.CropSpaces(trans[i][..pos]);
            valuestr = trans[i][(pos+1)..];
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

    public Pattern ParsePattern(string input)
    {
        string section;
        int begpos = input.IndexOf('#');
        if (begpos == -1)
            throw new ParseException("Parse error: not found any section");
        int endpos;
        var linedict = new Dictionary<NamedLine, List<NamedLineOption>>();

        try
        {
            while (begpos != input.Length)
            {
                endpos = input.IndexOfAny(" \n".ToCharArray(), begpos);
                section = this.CropSpaces(input[begpos..endpos]);
                begpos = input.IndexOf('#', endpos);
                if (begpos == -1)
                    begpos = input.Length;
                switch (section)
                {
                    case "#settings":
                        this.ParseSettings(input[endpos..begpos]);
                        break;
                    case "#start":
                        linedict = this.ParsePatternDict(input[endpos..begpos]);
                        break;
                    default:
                        throw new ParseException("Unknown section: " + section);
                }
            }
        }
        catch (ParseException e)
        {
            throw new ParseException("Parse error: " + e.Message);
        }
        
        return new Pattern(this.initline, linedict);
    }
}