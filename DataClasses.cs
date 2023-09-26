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

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (!(obj is NamedLine))
                return false;
            var ano = (NamedLine)obj;
            return (this.type == ano.type) && 
                StringComparer.OrdinalIgnoreCase.Equals(this.value, ano.value) && 
                StringComparer.OrdinalIgnoreCase.Equals(this.index, ano.index);
        }

        public override int GetHashCode()
        {
            return this.type.GetHashCode() ^
                StringComparer.OrdinalIgnoreCase.GetHashCode(this.value) ^
                StringComparer.OrdinalIgnoreCase.GetHashCode(this.index);
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

    class NamedLinesField
    {
        public NamedLine initline = new NamedLine(NamedLineTypes.NoteCount, "0", "");
        public List<NamedLine> value = new List<NamedLine>();
        public Pattern patt = new Pattern();
        public NamedLinesField() {}
        
    }
}