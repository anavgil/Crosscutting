namespace Crosscutting.Application.Messages.Base;

public interface IApiResponsePaged
{
    int PageNumber { get; set; }

     int TotalPages { get; set; }

     bool IsFirstPage { get; set; }

     bool IsLastPage{ get; set; }

     int Count{ get; set; }
}

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
    public bool IsSuccess { get; set; }
    public List<ApiError>? ErrorMessages { get; set; }
    public T? Data { get; set; }
}

public record ApiResponseBaseScalar<T> : IApiResponseBase, IApiResponseBaseScalar<T> where T : struct
{
    public bool IsSuccess { get; set; }
    public List<ApiError>? ErrorMessages { get; set; }
    public T? Data { get; set; }
}

public record ApiResponseBasePaged<T> : IApiResponseBase, IApiResponsePaged, IApiResponseBaseReadOnly<T> where T : class?
{
    public bool IsSuccess { get; set; }
    public List<ApiError>? ErrorMessages { get; set; }
    public T? Data { get; set; }
    public int PageNumber { get; set; } = 0;
    public int TotalPages { get; set; } = 0;
    public bool IsFirstPage { get; set; } = true;
    public bool IsLastPage { get; set; } = true;
    public int Count { get; set; } = 0;
}

public record ApiError(string Code, string Description) { }
