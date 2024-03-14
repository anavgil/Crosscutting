namespace Crosscutting.Application.Messages.Base;

public class ApiResponseBase<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public List<ApiError> ErrorMessages { get; set; }
}

public class ApiError(string code, string description)
{
    public string Code { get; } = code;
    public string Description { get; } = description;
}
