using System;
using System.Collections.Generic;

namespace mpg;
enum PlacementWay
{
    StreamStrong = 0,
    StreamWeak = 1,
    Random = 2
}

class NamedLineOption 
{
    public NamedLine line = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public int value;
    public PlacementWay way;

    public NamedLineOption(NamedLine line, int value, PlacementWay way)
    {
        this.line = line;
        this.value = value;
        this.way = way;
    }
}

class Pattern
{
    public NamedLine initline = new NamedLine();
    public NamedLineSetup defaultline = new NamedLineSetup(new NamedLine(), PlacementWay.StreamStrong);

    public Dictionary<NamedLine, List<NamedLineOption>> linedict = new Dictionary<NamedLine, List<NamedLineOption>>();
    private Random _rnd = new Random();
    public Pattern() 
    {
        this.linedict = new Dictionary<NamedLine, List<NamedLineOption>>();
        this.linedict.Add(new NamedLine(), 
                new List<NamedLineOption>() 
                    {new NamedLineOption(this.initline, 1, PlacementWay.StreamStrong)});
    }
    public Pattern(NamedLine initline, NamedLineSetup defaultline, Dictionary<NamedLine, List<NamedLineOption>> linedict)
    {
        this.initline = initline;
        this.defaultline = defaultline;
        this.linedict = linedict;
        this.linedict.Add(new NamedLine(), 
                        new List<NamedLineOption>() 
                            {new NamedLineOption(this.initline, 1, PlacementWay.StreamStrong)});
    }

    public NamedLineSetup NextNamedLineSetup(NamedLineSetup current)
    {
        List<NamedLineOption>? posnext;
        if (this.linedict.TryGetValue(current.line, out posnext))
        {
            int sum = 0;
            foreach (NamedLineOption item in posnext)
                sum += item.value;
            int point = this._rnd.Next(1, sum + 1);
            for (int i = 0; i < posnext.Count; i++)
            {
                if (point <= posnext[i].value)
                    return new NamedLineSetup(posnext[i].line, posnext[i].way);
                point -= posnext[i].value;
            }
            return new NamedLineSetup(new NamedLine(NamedLineTypes.String, "error get value", ""), PlacementWay.Random);
        }
        return this.defaultline;
    }
}