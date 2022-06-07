using System.Security.Claims;

namespace Todo.Core.Api.Startup.Security;

public interface IExternalClaimsToUserTransformer
{
    Todo.Core.Persistence.Entities.User ToUser(IEnumerable<Claim> claims);
}