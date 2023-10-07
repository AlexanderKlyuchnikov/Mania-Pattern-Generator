using System;

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
}