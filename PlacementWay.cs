using System;
using System.Collections.Generic;
using System.Linq;

namespace mpg;

enum WayType
{
    Strong = 0,
    Weak = 1
}

abstract class APlacementWay
{
    public WayType type = WayType.Strong;
    public abstract void Fill(params string[] args);
    public abstract List<int> GetPositions(List<string> strlines);
    public abstract override string ToString();
}

class StreamWay : APlacementWay
{
    public int distance = 1;
    public int offset = 0;
    public StreamWay() {}
    public StreamWay(WayType type, int distance, int offset)
    {
        this.type = type;
        this.distance = distance;
        this.offset = offset;
    }
    public override void Fill(params string[] str)
    {

    }
    public override List<int> GetPositions(List<string> strlines)
    {
        int pos;
        if (strlines.Count <= this.offset + this.distance)
            pos = 0;
        else
            pos = strlines.Count - (this.offset + this.distance);
        var result = new List<int>();
        bool valid;
        for (int i = 0; i < strlines.Last().Length; i++)
        {
            valid = true;
            for (int j = pos; j < pos + this.distance; j++)
            {
                if (strlines[j][i] == '1')
                {
                    valid = false;
                    break;
                }  
            }
            if (valid)
                result.Add(i);
        }
        return result;
    }

    public override string ToString()
    {
        return "Stream:" + this.type + ',' + this.distance + ',' + this.offset;
    }
}