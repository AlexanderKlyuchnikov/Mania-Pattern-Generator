using System;

namespace mpg;

class NamedLineOption 
{
    public NamedLine line = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public int value;
    public APlacementWay way;

    public NamedLineOption(NamedLine line, int value, APlacementWay way)
    {
        this.line = line;
        this.value = value;
        this.way = way;
    }
}

class NamedLineSetup
{
    public NamedLine line = new NamedLine();
    public APlacementWay way = new StreamWay();
    public NamedLineSetup() {}
    public NamedLineSetup(NamedLine line, APlacementWay way)
    {
        this.line = line;
        this.way = way;
    }
}

static class ForRandom
{
    public static Random rnd = new Random();
}
