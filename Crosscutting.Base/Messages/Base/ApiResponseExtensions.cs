using Crosscutting.Application.Dtos;

namespace Crosscutting.Application.Messages.Base;

public static class ApiResponseExtensions
{
    public static ApiResponseBase<T> ToApiResponseBase<T>(this FluentResults.IResult<T> result) where T : class?
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

                foreach (var reason in error.Reasons)
                {
                    responseBase.ErrorMessages.Add(new ApiError(string.Empty, reason.Message));
                }
            }
        }

        return responseBase;
    }

    public static ApiResponseBaseScalar<T> ToApiResponseBaseScalar<T>(this FluentResults.IResult<T> result) where T : struct
    {
        ApiResponseBaseScalar<T> responseBase = new()
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

                foreach (var reason in error.Reasons)
                {
                    responseBase.ErrorMessages.Add(new ApiError(string.Empty, reason.Message));
                }
            }
        }

        return responseBase;
    }

    public static ApiResponseBasePaged<IReadOnlyList<Y?>> ToApiResponseBasePaged<T, Y>(this FluentResults.IResult<T> result)
        where T : class?, IPagedResult<Y>
        where Y : class?
    {
        ApiResponseBasePaged<IReadOnlyList<Y?>> responseBase = new()
        {
            IsSuccess = result.IsSuccess,
            Data = result.ValueOrDefault.Items,
            ErrorMessages = [],
            PageNumber = result.ValueOrDefault.PageNumber,
            Count = result.ValueOrDefault.Count,
            IsFirstPage = result.ValueOrDefault.IsFirstPage,
            IsLastPage = result.ValueOrDefault.IsLastPage,
            TotalPages = result.ValueOrDefault.TotalPages,
        };

        if (result.IsFailed && result.Errors.Count != 0)
        {
            foreach (var error in result.Errors)
            {
                responseBase.ErrorMessages.Add(new ApiError(string.Empty, error.Message));

                foreach (var reason in error.Reasons)
                {
                    responseBase.ErrorMessages.Add(new ApiError(string.Empty, reason.Message));
                }
            }
        }

        return responseBase;
    }
}
