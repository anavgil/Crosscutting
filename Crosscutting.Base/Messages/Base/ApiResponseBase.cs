namespace Crosscutting.Application.Messages.Base;

public record ApiResponseBase<T> where T : class?
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public List<ApiError>? ErrorMessages { get; set; }
}

public record ApiError(string Code, string Description){}
