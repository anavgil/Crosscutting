namespace Crosscutting.Common.Exceptions;

[Serializable]
public class CustomValidationException : Exception
{
    public IEnumerable<ValidationErrorExceptionItem> Errors { get; private set; }
    public CustomValidationException(IEnumerable<ValidationErrorExceptionItem> errorCollection) : base(BuildErrorMessage(errorCollection))
    {
        Errors = errorCollection;
    }
    private static string BuildErrorMessage(IEnumerable<ValidationErrorExceptionItem> errors)
    {
        var arr = errors.Select(x => $"{Environment.NewLine} -- {x.Property}: {x.Error} Severity: {x.Severity.ToString()}");
        return "Validation failed: " + string.Join(string.Empty, arr);
    }
}


public class ValidationErrorExceptionItem
{
    public string Property { get; set; }
    public string Error { get; set; }
    public string Severity { get; set; }
}
