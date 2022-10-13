using System.Net.Http;
using Xunit;

namespace ParkIt.Test.Integration;

[CollectionDefinition("db1")]
public class ApiTest : IClassFixture<ParkItTestFactory>
{
    private readonly HttpClient _client;
    private readonly ParkItTestFactory _factory;

    public ApiTest(ParkItTestFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // [Fact]
    // public async Task GetAllParkingSpots()
    // {
    //     var response = await _client.GetFromJsonAsync<List<ParkingSpotResponse>>("/spots");
    //     var count = response.Count;
    //     Assert.Equal(5,count);
    // }

    // public void Dispose()
    // {
    //     using var scope = _factory.Services.CreateScope();
    //     var context = scope.ServiceProvider.GetRequiredService<ParkItDbContext>();
    //     context.Database.EnsureDeleted();
    // }
}