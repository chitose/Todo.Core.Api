using System.Security.Claims;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Api.Startup.Security;

public class TwitterClaimToUserTransformer : IExternalClaimsToUserTransformer
{
    public User ToUser(IEnumerable<Claim> claims)
    {
        var userName = claims.FirstOrDefault(c =>
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        var screenName =
            claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        var fullName = claims.FirstOrDefault(c => c.Type == "name");
        var emailClaim = claims.FirstOrDefault(c =>
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        var photoClaim = claims.FirstOrDefault(c => c.Type == "profile_image_url_https");
        var nameParts = fullName?.Value.Split(" ");
        return new User
        {
            UserName = userName?.Value,
            FirstName = nameParts.FirstOrDefault(),
            LastName = nameParts?.LastOrDefault(),
            Photo = photoClaim?.Value,
            Email = emailClaim?.Value,
            EmailConfirmed = true,
            DisplayName = screenName?.Value
        };
    }
}