namespace Crosscutting.Application.Messages.Base;

public static class ApiResponseExtensions
{
    public static ApiResponseBase<T> ToApiResponseBase<T>(this FluentResults.IResult<T> result)
    {
        ApiResponseBase<T> responseBase = new()
        {
            IsSuccess = result.IsSuccess,
            Data = result.ValueOrDefault,
            ErrorMessages = []
        };

        if (result.IsFailed && result.Errors.Count != 0)
        {
            foreach (var error in result.Errors)
            {
                responseBase.ErrorMessages.Add(new ApiError(string.Empty, error.Message));
            }
        }

        return responseBase;
    }
}
