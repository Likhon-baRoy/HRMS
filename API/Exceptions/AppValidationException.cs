namespace API.Exceptions;

public class AppValidationException(string message, Dictionary<string, string[]> errors) : Exception(message)
{
    public Dictionary<string, string[]> Errors { get; set; } = errors;
}
