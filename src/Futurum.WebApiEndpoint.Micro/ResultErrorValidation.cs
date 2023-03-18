using Futurum.Core.Linq;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// ResultError that for validation errors
/// </summary>
public class ResultErrorValidation : IResultErrorNonComposite
{
    internal ResultErrorValidation(IEnumerable<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    public IEnumerable<ValidationError> ValidationErrors { get; }

    /// <inheritdoc />
    public string GetErrorStringSafe() =>
        ValidationErrors.Select(TransformToErrorMessage)
                        .StringJoin(",");

    /// <inheritdoc />
    public string GetErrorString() =>
        GetErrorStringSafe();

    /// <inheritdoc />
    public ResultErrorStructure GetErrorStructureSafe()
    {
        var children = ValidationErrors.Select(TransformToErrorMessage)
                                       .Select(ResultErrorStructureExtensions.ToResultErrorStructure);

        return new ResultErrorStructure("Validation failure", children);
    }

    /// <inheritdoc />
    public ResultErrorStructure GetErrorStructure() =>
        GetErrorStructureSafe();

    private static string TransformToErrorMessage(ValidationError validationError) =>
        $"Validation failure for '{validationError.PropertyName}' with error : '{validationError.ErrorMessages.StringJoin(";")}'";
}