using System;

namespace mpg;

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

static class ForRandom
{
    public static Random rnd = new Random();
}
