using System.Security.Claims;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Api.Startup.Security;

public class GoogleClaimToUserTransformer : IExternalClaimsToUserTransformer
{
    public User ToUser(IEnumerable<Claim> claims)
    {
        var userName = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        var fullNameClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        var givenNameClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
        var surnameClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
        var emailClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        var photoClaim = claims.FirstOrDefault(c => c.Type == "urn:google:picture");
        return new User
        {
            UserName = userName?.Value,
            FirstName = surnameClaim?.Value,
            LastName = givenNameClaim?.Value,
            Photo = photoClaim?.Value,
            Email = emailClaim?.Value,
            EmailConfirmed = true,
            DisplayName = fullNameClaim?.Value
        };
    }
}