using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ParkIt.Contracts.Response;

namespace ParkIt.UI.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authState;
    private readonly ILocalStorageService _localStorage;

    private readonly JsonSerializerOptions _options;
    public AuthService(HttpClient httpClient, AuthenticationStateProvider authState, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authState = authState;
        _localStorage = localStorage;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<bool> Authenticate<T>(T request, string endpoint)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);
        if (!response.IsSuccessStatusCode) return false;
        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(_options);
        if (tokenResponse?.AccessToken == null) return false;
        await _localStorage.SetItemAsync("accessToken", tokenResponse.AccessToken);
        ((JwtAuthenticationStateProvider)_authState).NotifyUserAuthentication(tokenResponse.AccessToken);
        return true;

    }

    // public async Task<bool> Register(RegisterRequest request)
    // {
    //     var response = await _httpClient.PostAsJsonAsync("auth/register", request);
    //     if (response.IsSuccessStatusCode)
    //     {
    //         var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(_options);
    //         if (tokenResponse != null && tokenResponse.AccessToken != null)
    //         {
    //             await _localStorage.SetItemAsync("accessToken", tokenResponse.AccessToken);
    //             ((JwtAuthenticationStateProvider)_authState).NotifyUserAuthentication(tokenResponse.AccessToken);
    //             return true;
    //         }
    //     }
    //
    //     return false;
    // }

    public async Task Logout()
    {
        await _httpClient.PostAsync("auth/logout", null);
        await _localStorage.RemoveItemAsync("accessToken");
        ((JwtAuthenticationStateProvider)_authState).NotifyUserLogout();
    }
}