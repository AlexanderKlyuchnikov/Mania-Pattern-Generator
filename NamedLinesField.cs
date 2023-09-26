using System;
using System.Collections.Generic;

namespace mpg;
class NamedLinesField
{
    public NamedLine initline = new NamedLine(NamedLineTypes.NoteCount, "0", "");
    public List<NamedLine> value = new List<NamedLine>();
    public Pattern patt = new Pattern();
    public NamedLinesField() {}
    
}