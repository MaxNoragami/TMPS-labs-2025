namespace lab2.Exceptions;

public class InvalidResourceException : Exception
{
    public InvalidResourceException(string message) : base(message) { }
    public InvalidResourceException(string message, Exception innerException) : base(message, innerException) { }
}