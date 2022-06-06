using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Todo.Core.Api.Controllers;

[AllowAnonymous]
public class Auth : Controller
{
    [Route("auth/google")]
    [HttpGet]
    public IActionResult GoogleLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse"),
        };

        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [Route("auth/google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(x => new
        {
            x.Issuer,
            x.OriginalIssuer,
            x.Type,
            x.Value
        });
        return Json(claims);
    }
}