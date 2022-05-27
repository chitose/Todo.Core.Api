namespace Todo.Core.Common.Exception;

public class TodoException : System.Exception
{
    public TodoException(string msg) : base(msg)
    {
    }
}