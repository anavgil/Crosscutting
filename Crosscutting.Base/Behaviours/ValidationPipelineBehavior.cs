using Crosscutting.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Crosscutting.Application.Behaviours;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ValidationFailure[] validationFailures = await ValidateAsync(request, cancellationToken);

        if (validationFailures.Length == 0)
        {
            return await next();
        }

        //if (typeof(TResponse).IsGenericType &&
        //    typeof(TResponse).GetGenericTypeDefinition() == typeof(IResult<>))
        //{
        //    Type resultType = typeof(TResponse).GetGenericArguments()[0];

        //    MethodInfo failureMethod = typeof(Result<>)
        //        .MakeGenericType(resultType)
        //        .GetMethod(nameof(IResult<object>.ValidationFailure));

        //    if (failureMethod is not null)
        //    {
        //        return (TResponse)failureMethod.Invoke(
        //            null,
        //            [CreateValidationError(validationFailures)]);
        //    }
        //}
        //else if (typeof(TResponse) == typeof(Result))
        //{
        //    return (TResponse)(object)Result.Fail(CreateValidationError(validationFailures));
        //}

        //var arr = errors.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage} Severity: {x.Severity.ToString()}");
        var errorsItemCollection = validationFailures.Select(v => new ValidationErrorExceptionItem
        {
            Property = v.PropertyName,
            Error = v.ErrorMessage,
            Severity = v.Severity.ToString()
        });

        //throw new ValidationException(validationFailures);
        throw new CustomValidationException(errorsItemCollection);
    }

    private async Task<ValidationFailure[]> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(
                            validators.Select(validator => validator.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

        ValidationFailure[] validationFailures = [.. validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)];

        return validationFailures;
    }

    //private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
    //    new(validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray());
}
