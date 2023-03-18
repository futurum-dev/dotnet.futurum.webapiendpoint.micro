namespace Futurum.WebApiEndpoint.Micro.Sample.Security;

public static class AuthorizationConstants
{
    public static class ClaimType
    {
        public const string Permissions = "permissions"; // ERGHH - can't find a built in constant here (System.Security.Claims.ClaimTypes) for this - https://blog.joaograssi.com/posts/2021/asp-net-core-permission-based-authorization-middleware/
    }
}