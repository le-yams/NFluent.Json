namespace NFluent.Json.Exceptions;

/// <summary>
/// Thrown when attempting to use an invalid JSON Path.
/// </summary>
public class InvalidPathException : Exception
{
    /// <summary>
    /// The invalid path raising this error.
    /// </summary>
    public string InvalidPath { get; } = string.Empty;

    internal InvalidPathException(string invalidPath, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        InvalidPath = invalidPath;
    }

    private InvalidPathException() : base()
    {
    }

    private InvalidPathException(string? message) : base(message)
    {
    }

    private InvalidPathException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}