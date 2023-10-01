using System;
using System.Collections.Generic;
using System.Linq;

namespace mpg;

class NamedLineSetup
{
    public NamedLine line = new NamedLine();
    public PlacementWay way = 0;
    public NamedLineSetup() {}
    public NamedLineSetup(NamedLine line, PlacementWay way)
    {
        this.line = line;
        this.way = way;
    }
    public string DefString(string prev)
    {
        return this.line.DefString(prev, this.way);
    }
}

class NamedLinesField
{
    public List<NamedLineSetup> value = new List<NamedLineSetup>();
    public Pattern patt = new Pattern();
    public NamedLinesField() {}
    
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