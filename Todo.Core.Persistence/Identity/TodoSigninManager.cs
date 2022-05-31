using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Identity;

public class TodoSigninManager : SignInManager<User>
{
    public TodoSigninManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger, IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor,
        logger, schemes, confirmation)
    {
    }
}