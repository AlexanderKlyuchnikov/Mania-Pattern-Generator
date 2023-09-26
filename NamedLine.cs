using System;
using System.Collections.Generic;

namespace mpg;

enum NamedLineTypes
{
    NoteCount = 0
}

class NamedLine
{
    public NamedLineTypes type = NamedLineTypes.NoteCount;
    public string value = "";
    public string index = "";
    private Random _rnd = new Random();
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

    public string DefString(string prev)
    {
        if (this.type == NamedLineTypes.NoteCount)
        {
            int val = int.Parse(this.value);
            List<int> avpos = new List<int>();
            char[] result = new char[4] {'0','0','0','0'};
            for (int i = 0; i < 4; i++)
                if (prev[i] == '0')
                    avpos.Add(i);
            for (int i = 0; i < val; i++)
            {
                Random rndhelp = new Random(this._rnd.Next() + i);
                int setpos = rndhelp.Next(0, avpos.Count);
                result[avpos[setpos]] = '1';
                avpos.RemoveAt(setpos);
            }
            return new string(result);
        }
        else
            return "0000";
    }
}