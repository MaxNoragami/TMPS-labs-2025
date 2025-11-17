namespace lab2.Exceptions;

public class NoVideoFoundException : InvalidResourceException
{
    public NoVideoFoundException(string message) : base(message) { }
    public NoVideoFoundException(string message, Exception innerException) : base(message, innerException) { }
}