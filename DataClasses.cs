using System;
using System.Collections.Generic;

namespace mpg
{
    class Chart
    {
        public List<string> chartdata = new List<string>();
        public Chart() {}
    }

    enum NamedLineTypes
    {
        NoteCount = 0
    }

    enum PlacementWay
    {
        StreamStrong = 0
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

        //public static NamedLine emptyline = new NamedLine(NamedLineTypes.NoteCount, "0", "");

        public string DefString(string prev)
        {
            if (this.type == NamedLineTypes.NoteCount)
            {
                int val = int.Parse(this.value);
                List<int> avpos = new List<int>();
                char[] result = new char[4] {'0','0','0','0'};
                for (int i = 0; i < 4; i++)
                {
                    if (prev[i] == '0')
                    {
                        avpos.Add(i);
                        //Console.WriteLine(i);
                    }
                }
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

    class NamedLinesField
    {
        public NamedLine initline = new NamedLine(NamedLineTypes.NoteCount, "0", "");
        public List<NamedLine> value = new List<NamedLine>();
        //public ManiaTemplate mantemp;
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
    }
}