using System;
using System.Collections.Generic;

namespace mpg;

enum NamedLineTypes
{
    NoteCount = 0,
    String = 1
}

class NamedLine
{
    public NamedLineTypes type = NamedLineTypes.NoteCount;
    public string value = "0";
    public string index = "";
    private Random _rnd = new Random();
    public NamedLine() {}
    public NamedLine(NamedLineTypes type, string value, string index)
    {
        this.type = type;
        this.value = value;
        this.index = index;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is NamedLine))
            return false;
        var ano = (NamedLine)obj;
        return (this.type == ano.type) && 
            StringComparer.OrdinalIgnoreCase.Equals(this.value, ano.value) && 
            StringComparer.OrdinalIgnoreCase.Equals(this.index, ano.index);
    }

    public override int GetHashCode()
    {
        return this.type.GetHashCode() ^
            StringComparer.OrdinalIgnoreCase.GetHashCode(this.value) ^
            StringComparer.OrdinalIgnoreCase.GetHashCode(this.index);
    }

    public string DefString(string prev, PlacementWay way)
    {
        if (this.type == NamedLineTypes.NoteCount)
        {
            if (way == PlacementWay.Random)
            {
                int val = int.Parse(this.value);
                List<int> avpos = new List<int>() {0, 1, 2, 3};
                char[] result = new char[4] {'0','0','0','0'};
                for (int i = 0; i < val; i++)
                {
                    if (avpos.Count != 0)
                    {
                        Random rndhelp = new Random(this._rnd.Next() + i);
                        int setpos = rndhelp.Next(0, avpos.Count);
                        result[avpos[setpos]] = '1';
                        avpos.RemoveAt(setpos);
                    }
                    else
                        break;
                }
                return new string(result);
            }
            else
            {
                int val = int.Parse(this.value);
                List<int> avpos = new List<int>();
                char[] result = new char[4] {'0','0','0','0'};
                for (int i = 0; i < 4; i++)
                    if (prev[i] == '0')
                        avpos.Add(i);
                int counter = val;
                for (int i = 0; i < val; i++)
                {
                    if (avpos.Count != 0)
                    {
                        Random rndhelp = new Random(this._rnd.Next() + i);
                        int setpos = rndhelp.Next(0, avpos.Count);
                        result[avpos[setpos]] = '1';
                        avpos.RemoveAt(setpos);
                        counter--;
                    }
                    else
                        break;
                }
                if ((counter > 0) && (way == PlacementWay.StreamWeak))
                {
                    for (int i = 0; i < 4; i++)
                        if (result[i] == '0')
                            avpos.Add(i);
                    for (int i = 0; i < avpos.Count; i++)
                    {
                        if (avpos.Count != 0)
                        {
                            Random rndhelp = new Random(this._rnd.Next() + i);
                            int setpos = rndhelp.Next(0, avpos.Count);
                            result[avpos[setpos]] = '1';
                            avpos.RemoveAt(setpos);
                            counter--;
                        }
                        else
                            break;
                    }
                }
                return new string(result);
            }
        }
        else if (this.type == NamedLineTypes.String)
        {
            if (way == PlacementWay.StreamStrong)
            {
                char[] result = new char[4] {'0','0','0','0'};
                for (int i = 0; i < prev.Length; i++)
                {
                    if (prev[i] == '1')
                        result[i] = '0';
                    else
                        result[i] = this.value[i];
                }
                return new string(result);
            }
            return string.Copy(this.value);
        }
        else
            return "0000";
    }
}