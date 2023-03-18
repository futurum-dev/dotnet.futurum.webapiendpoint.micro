using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Micro;

public interface IValidationService<in T>
    where T : class
{
    Result Execute(T instance);
    Task<Result> ExecuteAsync(T instance);
}

public class ValidationService<T> : IValidationService<T>
    where T : class
{
    private readonly IFluentValidationService<T> _fluentValidationService;
    private readonly IDataAnnotationsValidationService _dataAnnotationsValidationService;

    public ValidationService(IFluentValidationService<T> fluentValidationService,
                             IDataAnnotationsValidationService dataAnnotationsValidationService)
    {
        _fluentValidationService = fluentValidationService;
        _dataAnnotationsValidationService = dataAnnotationsValidationService;
    }

    public Result Execute(T instance)
    {
        var fluentValidationResult = _fluentValidationService.Execute(instance);
        var dataAnnotationsValidationResult = _dataAnnotationsValidationService.Execute(instance);

        return Result.Combine(fluentValidationResult, dataAnnotationsValidationResult)
                     .Compensate(GetResultError);
    }

    public async Task<Result> ExecuteAsync(T instance)
    {
        var fluentValidationResult = await _fluentValidationService.ExecuteAsync(instance);
        var dataAnnotationsValidationResult = _dataAnnotationsValidationService.Execute(instance);

        return Result.Combine(fluentValidationResult, dataAnnotationsValidationResult)
                     .Compensate(GetResultError);
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