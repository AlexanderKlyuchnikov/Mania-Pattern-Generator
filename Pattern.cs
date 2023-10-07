using System;
using System.Collections.Generic;

using static mpg.ForRandom;

namespace mpg;

class Pattern
{
    public NamedLine initline = new();
    public Dictionary<NamedLine, List<NamedLineOption>> linedict = new Dictionary<NamedLine, List<NamedLineOption>>();
    public Pattern() 
    {
        this.linedict = new Dictionary<NamedLine, List<NamedLineOption>>();
        this.linedict.Add(new NamedLine(), 
                new List<NamedLineOption>() 
                    {new NamedLineOption(this.initline, 1, new StreamWay())});
    }
    public Pattern(NamedLine initline, Dictionary<NamedLine, List<NamedLineOption>> linedict)
    {
        this.initline = initline;
        this.linedict = linedict;
        this.linedict.Add(new NamedLine(), 
                        new List<NamedLineOption>() 
                            {new NamedLineOption(this.initline, 1, new StreamWay())});
    }

    public NamedLineSetup NextNamedLineSetup(NamedLineSetup current)
    {
        List<NamedLineOption>? posnext;
        if (this.linedict.TryGetValue(current.line, out posnext))
        {
            int sum = 0;
            foreach (NamedLineOption item in posnext)
                sum += item.value;
            int point = rnd.Next(1, sum + 1);
            for (int i = 0; i < posnext.Count; i++)
            {
                if (point <= posnext[i].value)
                    return new NamedLineSetup(posnext[i].line, posnext[i].way);
                point -= posnext[i].value;
            }
            return new NamedLineSetup(new NamedLine(NamedLineTypes.String, "error get value", ""), new StreamWay());
        }
        throw new GenerateException("Generate error: Not found transition for NamedLine: " 
            + "type = " + current.line.type + ", value = " + current.line.value + ", index =" + current.line.index);
    }
}