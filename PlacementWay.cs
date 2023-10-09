using System;
using System.Collections.Generic;
using System.Linq;

namespace mpg;

enum WayType
{
    Strong = 0,
    Weak = 1
}

abstract class APlacementWay
{
    public WayType type = WayType.Strong;
    public abstract void Fill(string[] args);
    public abstract List<int> GetPositions(List<string> strlines);
    //public abstract override string ToString();
}

class StreamWay : APlacementWay
{
    public int distance = 1;
    public int offset = 0;
    public StreamWay() {}
    public StreamWay(WayType type, int distance, int offset)
    {
        this.type = type;
        this.distance = distance;
        this.offset = offset;
    }
    public override void Fill(string[] str)
    {
        if (str.Length != 3)
            throw new ParseException("Wrong number of StreamWay arguments: " + str.Length.ToString() + " instead of 3");
        
        if (str[0].Length == 0)
            this.type = WayType.Strong;
        else
            this.type = str[0] switch
            {
                "Strong" => WayType.Strong,
                "Weak" => WayType.Weak,
                _ => throw new ParseException("Unknown StreamWay type (first argument): " + str[0]),
            };
        
        if (str[1].Length == 0)
            this.distance = 1;
        else
            if (int.TryParse(str[1], out int dist))
                if (dist > 0)
                    this.distance = dist;
                else
                    throw new ParseException("Not positive distance (second argument) for StreamWay: " + dist.ToString());
            else
                throw new ParseException("Not integer argument for StreamWay distance (second argument): " + str[1]);
        
        if (str[2].Length == 0)
            this.offset = 0;
        else
            if (int.TryParse(str[2], out int offs))
                if (offs >= 0)
                    this.offset = offs;
                else
                    throw new ParseException("Negative offset (third argument) for StreamWay: " + offs.ToString());
            else
                throw new ParseException("Not integer argument for StreamWay offset (third argument): " + str[2]);
    }
    public override List<int> GetPositions(List<string> strlines)
    {
        int pos;
        if (strlines.Count <= this.offset + this.distance)
            pos = 0;
        else
            pos = strlines.Count - (this.offset + this.distance);
        var result = new List<int>();
        bool valid;
        for (int i = 0; i < strlines.Last().Length; i++)
        {
            valid = true;
            for (int j = pos; j < pos + this.distance; j++)
            {
                if (strlines[j][i] == '1')
                {
                    valid = false;
                    break;
                }  
            }
            if (valid)
                result.Add(i);
        }
        return result;
    }

    public override string ToString()
    {
        return "Stream:" + this.type + ',' + this.distance + ',' + this.offset;
    }
}

class JackWay : APlacementWay
{
    public int distance = 1;
    public int offset = 0;
    public JackWay() {}
    public override void Fill(string[] args)
    {
        if (args.Length != 3)
            throw new ParseException("Wrong number of JackWay arguments: " + args.Length.ToString() + " instead of 3");
        
        if (args[0].Length == 0)
            this.type = WayType.Strong;
        else
            this.type = args[0] switch
            {
                "Strong" => WayType.Strong,
                "Weak" => WayType.Weak,
                _ => throw new ParseException("Unknown JackWay type (first argument): " + args[0]),
            };
        
        if (args[1].Length == 0)
            this.distance = 1;
        else
            if (int.TryParse(args[1], out int dist))
                if (dist > 0)
                    this.distance = dist;
                else
                    throw new ParseException("Not positive distance (second argument) for JackWay: " + dist.ToString());
            else
                throw new ParseException("Not integer argument for JackWay distance (second argument): " + args[1]);
        
        if (args[2].Length == 0)
            this.offset = 0;
        else
            if (int.TryParse(args[2], out int offs))
                if (offs >= 0)
                    this.offset = offs;
                else
                    throw new ParseException("Negative offset (third argument) for JackWay: " + offs.ToString());
            else
                throw new ParseException("Not integer argument for JackWay offset (third argument): " + args[2]);
    }

    public override List<int> GetPositions(List<string> strlines)
    {
        int pos;
        if (strlines.Count <= this.offset + this.distance)
            pos = 0;
        else
            pos = strlines.Count - (this.offset + this.distance);
        var result = new List<int>();
        bool valid;
        for (int i = 0; i < strlines.Last().Length; i++)
        {
            valid = true;
            for (int j = pos; j < pos + this.distance; j++)
            {
                if (strlines[j][i] == '0')
                {
                    valid = false;
                    break;
                }  
            }
            if (valid)
                result.Add(i);
        }
        return result;
    }
}

class SetWay : APlacementWay
{
    public List<int>? avpos = null;
    public SetWay() {}
    public override void Fill(string[] args)
    {
        if (args.Length != 2)
            throw new ParseException("Wrong number of SetWay arguments: " + args.Length.ToString() + " instead of 2");
        if (args[0].Length == 0)
            this.type = WayType.Strong;
        else
            type = args[0] switch
            {
                "Strong" => WayType.Strong,
                "Weak" => WayType.Weak,
                _ => throw new ParseException("Unknown SetWay type (first argument): " + args[0]),
            };
        if ((args[1].First() != '{') || (args[1].Last() != '}'))
            throw new ParseException("Second argument in SetWay should be in \"{...}\": " + args[1]);
        string[] lst = PatternParser.SplitAttributes(args[1][1..(args[1].Length - 1)]);
        this.avpos = new List<int>();
        foreach (var item in lst)
        {
            if (int.TryParse(item, out int pos))
                this.avpos.Add(pos - 1);
            else
                throw new ParseException("Not integer argument for Setway positions (argument in 2nd argument): " + item);
        }
    }
    public override List<int> GetPositions(List<string> strlines)
    {
        if (this.avpos is null)
            return new List<int>(4){0, 1, 2, 3};
        return new List<int>(this.avpos);
    }
}