using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Crosscutting.Application.Messages.Base;

public static class ApiResponseExtensions
{
    public static ApiResponseBase<T> ToApiResponseBase<T>(this FluentResults.Result<T> result)
    {
        ApiResponseBase<T> responseBase = new()
        {
            IsSuccess = result.IsSuccess,
            Data = result.ValueOrDefault
        };

        if (result.IsFailed && result.Errors.Count != 0) { }
        {
            foreach (var error in result.Errors)
            {
                responseBase.ErrorMessages.Add(new ApiError(string.Empty, error.Message));
            }
        }

        return responseBase;
    }
}
