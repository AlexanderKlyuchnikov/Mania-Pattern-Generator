using System;
using System.Collections.Generic;

namespace mpg;
enum PlacementWay
{
    StreamStrong = 0
}

class NamedLineWay 
{
    public NamedLine line = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public uint value;
    public PlacementWay way;
}

class Pattern
{
    public NamedLine initline = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public NamedLine defaultline = new NamedLine(NamedLineTypes.NoteCount, "0", "");

    public Dictionary<NamedLine, NamedLineWay> linedict = new Dictionary<NamedLine, NamedLineWay>();
    public Pattern() {}
    public Pattern(NamedLine initline, NamedLine defaultline, Dictionary<NamedLine, NamedLineWay> linedict)
    {
        this.initline = initline;
        this.defaultline = defaultline;
        this.linedict = linedict;
    }

    public NamedLine NextNamedLine(NamedLine current)
    {
        if (!this.linedict.ContainsKey(current))
            return this.defaultline;
        return new NamedLine(0, "4", "");
    }
}