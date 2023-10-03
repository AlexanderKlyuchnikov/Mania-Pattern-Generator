using System;
using System.Collections.Generic;
using System.Linq;

namespace mpg;

class NamedLinesField
{
    public List<NamedLineSetup> value = new List<NamedLineSetup>();
    public Pattern patt = new Pattern();
    public NamedLinesField() {}
    public NamedLinesField(Pattern pattern)
    {
        this.value = new List<NamedLineSetup>();
        this.patt = pattern;
    }
    
    public void GenerateValue(int length)
    {
        NamedLineSetup last = new NamedLineSetup();
        this.value.Clear();
        for (int i = 0; i < length; i++)
        {
            last = this.patt.NextNamedLineSetup(last);
            this.value.Add(last);
        }
    }
    public string GetString()
    {
        List<string> strlines = new List<string>() {"0000"};
        for (int i = 0; i < this.value.Count; i++)
            strlines.Add(this.value[i].DefString(strlines.Last()));
        strlines.RemoveAt(0);
        return string.Join("\n", strlines);
    }
}