using System;
using System.Collections.Generic;

namespace mpg;
enum PlacementWay
{
    StreamStrong = 0,
    StreamWeak = 1
}

class NamedLineWay 
{
    public NamedLine line = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public int value;
    public PlacementWay way;

    public NamedLineWay(NamedLine line, int value, PlacementWay way)
    {
        this.line = line;
        this.value = value;
        this.way = way;
    }
}

class Pattern
{
    public NamedLine initline = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public NamedLine defaultline = new NamedLine(NamedLineTypes.NoteCount, "0", "");

    public Dictionary<NamedLine, List<NamedLineWay>> linedict = new Dictionary<NamedLine, List<NamedLineWay>>();
    private Random _rnd = new Random();
    public Pattern() {}
    public Pattern(NamedLine initline, NamedLine defaultline, Dictionary<NamedLine, List<NamedLineWay>> linedict)
    {
        this.initline = initline;
        this.defaultline = defaultline;
        this.linedict = linedict;
    }

    public NamedLine NextNamedLine(NamedLine current, out PlacementWay way)
    {
        List<NamedLineWay>? posnext;
        way = PlacementWay.StreamStrong;
        if (this.linedict.TryGetValue(current, out posnext))
        {
            int sum = 0;
            foreach (NamedLineWay item in posnext)
                sum += item.value;
            int point = this._rnd.Next(1, sum + 1);
            for (int i = 0; i < posnext.Count; i++)
            {
                if (point <= posnext[i].value)
                {
                    way = posnext[i].way;
                    return posnext[i].line;
                }
                    
                point -= posnext[i].value;
            }
            return new NamedLine();
        }
        return this.defaultline;
    }
}