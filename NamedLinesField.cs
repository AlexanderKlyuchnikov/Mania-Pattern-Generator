using System;
using System.Collections.Generic;
using System.Linq;

using static mpg.ForRandom;

namespace mpg;

class NamedLinesField
{
    public List<NamedLineSetup> value = new();
    public Pattern patt = new();
    public NamedLinesField() {}
    public NamedLinesField(Pattern pattern)
    {
        this.value = new List<NamedLineSetup>();
        this.patt = pattern;
    }
    
    public void GenerateValue(int length)
    {
        NamedLineSetup last = new();
        this.value.Clear();
        for (int i = 0; i < length; i++)
        {
            last = this.patt.NextNamedLineSetup(last);
            this.value.Add(last);
        }
    }

    public void AddString(List<string> strlines, int number)
    {
        List<int> avpos = this.value[number].way.GetPositions(strlines);
        List<int> usedpos = new();
        char[] nstr = "0000".ToCharArray();
        int setpos;
        if (this.value[number].line.type == NamedLineTypes.NoteCount)
        {
            int count = int.Parse(this.value[number].line.value);
            while ((count > 0) && (avpos.Count > 0))
            {
                setpos = rnd.Next(0, avpos.Count);
                nstr[avpos[setpos]] = '1';
                avpos.RemoveAt(setpos);
                usedpos.Add(setpos);
                count--;
            }
            if ((count > 0) && (this.value[number].way.type == WayType.Weak))
            {
                avpos = new List<int>() {0, 1, 2, 3};
                avpos = avpos.Except(usedpos).ToList<int>();
                while ((count > 0) && (avpos.Count > 0))
                {
                    setpos = rnd.Next(0, avpos.Count);
                    nstr[avpos[setpos]] = '1';
                    avpos.RemoveAt(setpos);
                    count--;
                }
            }
        }
        else if (this.value[number].line.type == NamedLineTypes.String)
        {
            if (this.value[number].way.type == WayType.Weak)
                nstr = this.value[number].line.value.ToCharArray();
            else
                foreach (int i in avpos)
                    nstr[i] = this.value[number].line.value[i];
        }
        strlines.Add(new string(nstr));
    }

    public string GetString()
    {
        List<string> strlines = new() {"0000"};
        for (int i = 0; i < this.value.Count; i++)
            this.AddString(strlines, i);
        strlines.RemoveAt(0);
        return string.Join("\n", strlines);
    }
}