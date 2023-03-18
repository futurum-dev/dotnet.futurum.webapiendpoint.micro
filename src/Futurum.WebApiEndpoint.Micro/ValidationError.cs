namespace Futurum.WebApiEndpoint.Micro;

public record ValidationError(string PropertyName, IEnumerable<string> ErrorMessages);