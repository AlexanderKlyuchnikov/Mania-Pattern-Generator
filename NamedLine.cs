using System;
using System.Collections.Generic;
using static mpg.ForRandom;

namespace mpg;

class NamedLine
{
    public NamedLineTypes type = NamedLineTypes.NoteCount;
    public string value = "0";
    public string index = "";
    public NamedLine() {}
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

    public string DefString(string prev, PlacementWay way)
    {
        char[] result = new char[4] {'0','0','0','0'};
        int counter;
        switch ((this.type, way))
        {
            case (NamedLineTypes.NoteCount, PlacementWay.Random):
                counter = int.Parse(this.value);
                SetNotesByCount(ref counter, ref result);
                break;
            case (NamedLineTypes.NoteCount, PlacementWay.StreamStrong):
                counter = int.Parse(this.value);
                SetStreamNotesByCount(prev, ref counter, ref result);
                break;
            case (NamedLineTypes.NoteCount, PlacementWay.StreamWeak):
                counter = int.Parse(this.value);
                SetStreamNotesByCount(prev, ref counter, ref result);
                if (counter > 0)
                    SetNotesByCount(ref counter, ref result);
                break;
            case (NamedLineTypes.String, PlacementWay.Random):
                SetNotesByString(prev, ref result);
                break;
            case (NamedLineTypes.String, PlacementWay.StreamStrong):
                SetStreamNotesByString(prev, ref result);
                break;
            case (NamedLineTypes.String, PlacementWay.StreamWeak):
                SetNotesByString(prev, ref result);
                break;
            default:
                result = new char[5] {'e', 'r', 'r', 'o', 'r'};
                break;
        }
        return new string(result);
    }

    private void SetNotesByCount(ref int counter, ref char[] result)
    {
        List<int> avpos = new List<int>(4);
        for (int i = 0; i < 4; i++)
            if (result[i] == '0')
                avpos.Add(i);
        while ((counter != 0) && (avpos.Count != 0))
        {
            //Random rndhelp = new Random(this._rnd.Next());
            int setpos = rnd.Next(0, avpos.Count);
            result[avpos[setpos]] = '1';
            avpos.RemoveAt(setpos);
            counter--;
        }
    }

    private void SetNotesByString(string prev, ref char[] result)
    {
        for (int i = 0; i < prev.Length; i++)
            result[i] = this.value[i];
    }

    private void SetStreamNotesByCount(string prev, ref int counter, ref char[] result)
    {
        List<int> avpos = new List<int>(4);
        for (int i = 0; i < 4; i++)
            if ((prev[i] == '0') && (result[i] == '0'))
                avpos.Add(i);
        while ((counter != 0) && (avpos.Count != 0))
        {
            //Random rndhelp = new Random(this._rnd.Next());
            int setpos = rnd.Next(0, avpos.Count);
            result[avpos[setpos]] = '1';
            avpos.RemoveAt(setpos);
            counter--;
        }
    }

    private void SetStreamNotesByString(string prev, ref char[] result)
    {
        for (int i = 0; i < prev.Length; i++)
            if (prev[i] == '1')
                result[i] = '0';
            else
                result[i] = this.value[i];
    }
}