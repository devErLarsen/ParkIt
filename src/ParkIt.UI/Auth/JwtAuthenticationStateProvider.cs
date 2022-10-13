using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ParkIt.UI.Auth;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _authState;
    public JwtAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("accessToken");
        return string.IsNullOrWhiteSpace(token) ? _authState :
            // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "ParkItApi")));
    }

    public void NotifyUserAuthentication(string token)
    {
        // var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "jwtAuthType"));
        // var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        var authState =
            Task.FromResult(new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "ParkItApi"))));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_authState);
        NotifyAuthenticationStateChanged(authState);
    }

    // private async Task<User> GetUserAsync()
    // {
    //     var jwt = await _localStorage.GetItemAsync<string>("accesstoken");
    //     if(jwt is null) return null;

    //     var user = await _httpClient.GetFromJsonAsync<User>("auth/me");

    //     if(user is null) return null;

    //     return user;
    // }
}