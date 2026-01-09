namespace Jobify.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation errors occurred.") =>
        Errors = new Dictionary<string, string[]>();

    public ValidationException(List<ValidationFailure> failures)
        : this()
    {
        IEnumerable<string> propertyNames = failures
            .Select(e => e.PropertyName)
            .Distinct();

        foreach (string propertyName in propertyNames)
        {
            string[] propertyFailures = failures
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            Errors.Add(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Errors { get; }
}
