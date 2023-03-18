using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Futurum.WebApiEndpoint.Micro.Sample.Security;

[WebApiEndpoint("security")]
public class SecurityLoginWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("login", LoginHandler);
    }

    private static Results<Ok<string>, UnauthorizedHttpResult> LoginHandler(HttpContext context, IConfiguration configuration, string username, string password, bool setPermission, bool setClaim, bool setRole)
    {
        if (username != "user1" || password != "password1")
        {
            return TypedResults.Unauthorized();
        }

        var claims = new List<Claim>();

        if (setPermission)
        {
            claims.Add(new Claim(AuthorizationConstants.ClaimType.Permissions, Authorization.Permission.Admin));
        }

        if (setClaim)
        {
            claims.Add(new Claim(Authorization.ClaimType.Admin, Authorization.Claim.Admin));
        }

        if (setRole)
        {
            claims.Add(new Claim(ClaimTypes.Role, Authorization.Role.Admin));
        }

        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];
        var jwtKey = configuration["Jwt:Key"];

        var claimsIdentity = new ClaimsIdentity(claims);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            Subject = claimsIdentity,
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));

        return TypedResults.Ok(stringToken);
    }
}