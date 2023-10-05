using System;

namespace mpg;

class MPGException : Exception
{ }

class ParseException : MPGException
{
    private string? _message;
    new public string? Message 
    {get 
        {return _message;}
     set 
        {_message = value;}}

    public ParseException()
    {
        this.Message = "";
    }

    public ParseException(string? mes)
    {
        this.Message = mes;
    }
    public override string ToString()
    {
        return "MPGen exception: " + Message; 
    }
}