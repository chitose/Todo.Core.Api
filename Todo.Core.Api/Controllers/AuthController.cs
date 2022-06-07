using AspNet.Security.OAuth.Twitter;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Api.Startup.Security;
using Todo.Core.Domain.Dto;
using Todo.Core.Service.User;

namespace Todo.Core.Api.Controllers;

[AllowAnonymous]
[Route("auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly ILifetimeScope _scope;
    private readonly IMapper _mapper;

    public AuthController(IUserService userService, ILifetimeScope scope, IMapper mapper)
    {
        _userService = userService;
        _scope = scope;
        _mapper = mapper;
    }

    /// <summary>
    /// Login with Google OAuth
    /// </summary>
    /// <returns><see cref="UserDto"/></returns>
    [HttpGet]
    [Route("google")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public IActionResult GoogleLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse"),
        };

        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet]
    [Route("google-response")]
    public Task<UserDto?> GoogleResponse()
    {
        return HanldeLoginResponse(GoogleDefaults.AuthenticationScheme, "google");
    }

    [HttpGet]
    [Route("twitter")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public IActionResult TwitterLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("TwitterResponse")
        };

        return Challenge(props, TwitterAuthenticationDefaults.AuthenticationScheme);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet]
    [Route("twitter-response")]
    public Task<UserDto?> TwitterResponse()
    {
        return HanldeLoginResponse(TwitterAuthenticationDefaults.AuthenticationScheme, "twitter");
    }
    
    private async Task<UserDto?> HanldeLoginResponse(string authScheme, string provider)
    {
        var result = await HttpContext.AuthenticateAsync(authScheme);

        if (result.Succeeded)
        {
            var transformer = _scope.ResolveNamed<IExternalClaimsToUserTransformer>(provider);
            var userInfo = transformer.ToUser(result.Principal.Identities.FirstOrDefault().Claims);

            var user = await _userService.GetUserByUserName(userInfo.UserName);
            if (user == null)
            {
                user = await _userService.CreateExtUser(userInfo);
            }

            await _userService.SignIn(user, CookieAuthenticationDefaults.AuthenticationScheme);

            return _mapper.Map<UserDto>(user);
        }

        if (result.Failure != null)
        {
            throw result.Failure;
        }

        return null;
    }
}