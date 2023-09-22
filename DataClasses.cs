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

    class NamedLine
    {
        public NamedLineTypes type = NamedLineTypes.NoteCount;
        public string value = "";
        public string index = "";
        public NamedLine(NamedLineTypes type, string value, string index)
        {
            this.type = type;
            this.value = value;
            this.index = index;
        }
    }
}