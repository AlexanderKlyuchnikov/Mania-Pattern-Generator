using System;
using System.Collections.Generic;

namespace mpg;
class NamedLinesField
{
    public NamedLine initline = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public List<NamedLine> value = new List<NamedLine>();
    public Pattern patt = new Pattern();
    public List<PlacementWay> ways = new List<PlacementWay>() {0};
    public NamedLinesField() {}
    
    public void GenerateValue(int length)
    {
        NamedLine last = this.initline;
        this.value.Clear();
        this.ways.Clear();
        this.value.Add(last);
        PlacementWay way = PlacementWay.StreamStrong;
        for (int i = 1; i < length; i++)
        {
            last = this.patt.NextNamedLine(last, out way);
            this.ways.Add(way);
            this.value.Add(last);
        }
    }
}