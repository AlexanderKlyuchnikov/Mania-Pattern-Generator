using System;

namespace mpg;

class MPGException : Exception
{
    private string? _message;
    new public string? Message 
    {get 
        {return _message;}
     set 
        {_message = value;}}
    
    public MPGException()
    {
        this.Message = "";
    }

    public MPGException(string? mes)
    {
        this.Message = mes;
    }
    
    public override string ToString()
    {
        return "MPGen exception: " + Message; 
    }
}

class ParseException : MPGException
{
    public ParseException() { }

    public ParseException(string? mes)
    {
        this.Message = mes;
    }
}

class GenerateException : MPGException
{
    public GenerateException() {}
    public GenerateException(string? mes)
    {
        this.Message = mes;
    }
}