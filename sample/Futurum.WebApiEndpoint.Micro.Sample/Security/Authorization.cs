using System.Security.Claims;

namespace Futurum.WebApiEndpoint.Micro.Sample.Security;

public static class Authorization
{
    public static class Permission
    {
        public const string Admin = "admin-permission";
    }

    public static class Claim
    {
        public const string Admin = "admin-claim";
    }

    public static class ClaimType
    {
        public const string Admin = ClaimTypes.System;
    }

    public static class Role
    {
        public const string Admin = "admin-role";
    }
}