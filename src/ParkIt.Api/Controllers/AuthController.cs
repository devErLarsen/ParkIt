using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ParkIt.Application.Services;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace ParkIt.Api.Controllers;
[Authorize]
[Route("auth")]
public class AuthController : BaseController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("secret")]
    public ActionResult<string> Secret()
    {
        return Ok("TIHI");
    }

    // [HttpGet("me")]
    // public async Task<ActionResult<UserResponse>> GetMe()
    //     => await _userService.GetCurrentUser(User.FindFirstValue("IdentityId"));

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
    {
        var tokens = await _userService.Login(request);
        SetCookie(tokens.RefreshToken, tokens.RefreshExpires);
        return Ok(new { tokens.AccessToken });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<TokenResponse>> Register(RegisterRequest request)
    {
        var tokens = await _userService.Register(request);
        SetCookie(tokens.RefreshToken, tokens.RefreshExpires);
        return Ok(new { tokens.AccessToken });

    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var identityId = User.FindFirstValue("IdentityId");
        await _userService.Logout(identityId);
        
        SetCookie("", DateTime.Now.AddDays(-1));
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("refresh-token")]
    public async Task<ActionResult<TokenResponse>> RefreshToken()
    {
        var tokenHeader = Request.Headers[HeaderNames.Authorization];
        _ = AuthenticationHeaderValue.TryParse(tokenHeader, out var headerValue);
        var accessToken = headerValue?.Parameter;
        var refreshToken = Request.Cookies["refresh-token"];
        var response = await _userService.RefreshToken(accessToken, refreshToken);
        SetCookie(response.RefreshToken, response.RefreshExpires);
        return Ok(new TokenResponse { AccessToken = response.AccessToken });
    }

    private void SetCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires
        };
        HttpContext.Response.Cookies.Append("refresh-token", refreshToken, cookieOptions);
    }

}