namespace Jobify.Application.Common.Exceptions;

public class NullDataException : Exception
{
    public NullDataException(string message) : base(message)
    {
    }
}
