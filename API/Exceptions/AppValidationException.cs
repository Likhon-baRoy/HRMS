namespace API.Exceptions;

public class AppValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; set; }

    public AppValidationException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}