using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ParkIt.Contracts.Response;

namespace ParkIt.UI.Auth;

public class JwtMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _client;
    public JwtMessageHandler(ILocalStorageService localStorage, IHttpClientFactory factory)
    {
        _localStorage = localStorage;
        _client = factory.CreateClient();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwt = await _localStorage.GetItemAsync<string>("accessToken");
        if (jwt is null)
            return await base.SendAsync(request, cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode is not (HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)) return response;
        try
        {
            var refreshRequest = new HttpRequestMessage();
            refreshRequest.RequestUri = new Uri("http://localhost:5000/auth/refresh-token");
            refreshRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var refresh =
                await _client.SendAsync(refreshRequest,  cancellationToken);
            var tokenResponse =
                await refresh.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
            if (tokenResponse is { AccessToken: { } })
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                await _localStorage.SetItemAsync("accessToken", tokenResponse.AccessToken, cancellationToken);
            }
            response = await base.SendAsync(request, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return response;
    }
}
