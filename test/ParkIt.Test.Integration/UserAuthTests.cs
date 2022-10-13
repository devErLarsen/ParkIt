using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using Xunit;
using Xunit.Extensions.Ordering;

namespace ParkIt.Test.Integration;

[CollectionDefinition("db1")]
public class UserAuthTests : IClassFixture<ParkItTestFactory>
{
    private readonly HttpClient _client;
    private readonly ParkItTestFactory _factory;
    private static string _token = string.Empty;
    private const string _email = "TestUser@testing.com";
    private const string _password = "Password123";

    public UserAuthTests(ParkItTestFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Theory, Order(1)]
    [InlineData("TestUser", _email, "", _password)]
    public async Task Register_should_return_ok_response_and_include_token(string name, string email, string role, string password)
    {
        var registerRequest = new RegisterRequest()
        {
            Name = name,
            Email = email,
            Role = role,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/auth/register", registerRequest);

        var token = response.Content.ReadFromJsonAsync<TokenResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(token);
    }

    [Theory, Order(2)]
    [InlineData(_email, _password)]
    public async Task Login_should_return_ok_response_and_include_token(string email, string password)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var token = await response.Content.ReadFromJsonAsync<TokenResponse>();

        _token = token?.AccessToken;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(token);
    }

    [Fact, Order(3)]
    public async Task Request_should_return_unauthorized()
    {
        var response = await _client.GetAsync("auth/secret");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact, Order(4)]
    public async Task Request_should_be_authorized()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        var response = await _client.GetAsync("auth/secret");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact, Order(5)]
    public async Task Request_should_return_valid_response()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        var employeeResponse = await _client.GetFromJsonAsync<EmployeeResponse>("employees/me");

        Assert.NotNull(employeeResponse);
    }

}