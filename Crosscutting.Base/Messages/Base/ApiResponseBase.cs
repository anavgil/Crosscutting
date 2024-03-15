namespace Crosscutting.Application.Messages.Base;

public interface IApiResponseBase
{
    bool IsSuccess { get; set; }
    List<ApiError>? ErrorMessages { get; set; }
}

public interface IApiResponseBaseReadOnly<T> where T : class?
{
    T? Data { get; set; }
}

public interface IApiResponseBaseScalar<T> where T : struct
{
    T? Data { get; set; }
}

public record ApiResponseBase<T> : IApiResponseBase, IApiResponseBaseReadOnly<T> where T : class?
{
    public bool IsSuccess { get;  set; }
    public List<ApiError>? ErrorMessages { get;  set; }
    public T? Data { get; set; }
}

public record ApiResponseBaseScalar<T> : IApiResponseBase, IApiResponseBaseScalar<T> where T : struct
{
    public bool IsSuccess { get; set; }
    public List<ApiError>? ErrorMessages { get; set; }
    public T? Data { get; set; }
}

public record ApiError(string Code, string Description){}
