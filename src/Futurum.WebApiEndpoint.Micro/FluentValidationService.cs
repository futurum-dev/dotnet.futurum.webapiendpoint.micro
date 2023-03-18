using FluentValidation;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Micro;

public interface IFluentValidationService<in T>
    where T : class
{
    Result Execute(T instance);
    Task<Result> ExecuteAsync(T instance);
}

public class FluentValidationService<T> : IFluentValidationService<T>
    where T : class
{
    private readonly IValidator<T>[] _validators;

    public FluentValidationService(IEnumerable<IValidator<T>> validators)
    {
        _validators = validators.ToArray();
    }

    public Result Execute(T instance) =>
        _validators.Length switch
        {
            0 => Result.Ok(),
            1 => Validate(_validators[0], instance),
            _ => Validate(_validators, instance)
        };

    public Task<Result> ExecuteAsync(T instance) =>
        _validators.Length switch
        {
            0 => Result.OkAsync(),
            1 => ValidateAsync(_validators[0], instance),
            _ => ValidateAsync(_validators, instance)
        };

    private static Result Validate(IValidator<T> validator, T instance)
    {
        var validationResult = validator.Validate(instance);

        if (validationResult.IsValid)
        {
            return Result.Ok();
        }

        var validationErrors = validationResult
                               .Errors
                               .Select(validationFailure => new ValidationError(validationFailure.PropertyName, new[] { validationFailure.ErrorMessage }));

        return Result.Fail(validationErrors.ToResultError());
    }

    private static Result Validate(IEnumerable<IValidator<T>> validators, T instance)
    {
        var validationResults = validators.FlatMap(validator => Validate(validator, instance));

        return validationResults.Switch(Result.Ok, GetResultError);
    }

    private static async Task<Result> ValidateAsync(IValidator<T> validator, T instance)
    {
        var validationResult = await validator.ValidateAsync(instance);

        if (validationResult.IsValid)
        {
            return Result.Ok();
        }

        var validationErrors = validationResult
                               .Errors
                               .Select(validationFailure => new ValidationError(validationFailure.PropertyName, new[] { validationFailure.ErrorMessage }));

        return Result.Fail(validationErrors.ToResultError());
    }

    private static async Task<Result> ValidateAsync(IEnumerable<IValidator<T>> validators, T instance)
    {
        var validationResults = await validators.FlatMapAsync(validator => ValidateAsync(validator, instance));

        return validationResults.Switch(Result.Ok, GetResultError);
    }

    private static Result GetResultError(IResultError error)
    {
        if (error is ResultErrorComposite resultErrorComposite)
        {
            var groupedByPropertyName = resultErrorComposite.Children
                                                            .SelectMany(TransformValidationResultError)
                                                            .GroupBy(x => x.PropertyName)
                                                            .Select(grouping => new ValidationError(grouping.Key, grouping.Select(x => x.ErrorMessage).ToList()));

            return Result.Fail(groupedByPropertyName.ToResultError());
        }

        return Result.Fail(error);
    }

    private static IEnumerable<(string PropertyName, string ErrorMessage)> TransformValidationResultError(IResultError childError)
    {
        if (childError is ResultErrorValidation resultErrorValidation)
        {
            foreach (var validationError in resultErrorValidation.ValidationErrors)
            {
                foreach (var errorMessage in validationError.ErrorMessages)
                {
                    yield return (validationError.PropertyName, errorMessage);
                }
            }
        }
    }
}