using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ApiTests : IClassFixture<CustomWebApplicationFactory<TradeManagementApp.API.Startup>>
{
    private readonly HttpClient _client;

    public ApiTests(CustomWebApplicationFactory<TradeManagementApp.API.Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(Skip="True")]
    public async Task GetAllAccounts_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/accounts");
        response.EnsureSuccessStatusCode();
    }

      [Fact (Skip="True")]
    public async Task GetAccountById_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/accounts/1");
        response.EnsureSuccessStatusCode();
    }

        [Fact (Skip="True")]
    public async Task CreateAccount_ReturnsSuccessStatusCode()
    {
        var content = new StringContent("{\"firstName\":\"John\",\"lastName\":\"Doe\"}", Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/accounts", content);
        response.EnsureSuccessStatusCode();
    }

       [Fact (Skip="True")]
    public async Task UpdateAccount_ReturnsSuccessStatusCode()
    {
        var content = new StringContent("{\"id\":1,\"firstName\":\"John\",\"lastName\":\"Doe\"}", Encoding.UTF8, "application/json");
        var response = await _client.PutAsync("/api/accounts/1", content);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteAccount_ReturnsSuccessStatusCode()
    {
        var response = await _client.DeleteAsync("/api/accounts/1");
        response.EnsureSuccessStatusCode();
    }

    // Add similar tests for the Trades endpoints

    [Fact]
    public async Task GetAllTrades_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/trades");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetTradeById_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/trades/1");
        response.EnsureSuccessStatusCode();
    }

    [Fact (Skip="True")]
        public async Task CreateTrade_ReturnsSuccessStatusCode()
    {
        var content = new StringContent("{\"accountId\":1,\"securityCode\":\"APL\",\"timestamp\":\"2025-02-10T00:00:00Z\",\"amount\":100,\"buyOrSell\":\"Buy\",\"status\":0}", Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/trades", content);
        response.EnsureSuccessStatusCode();
    }

    [Fact (Skip="True")]
    public async Task UpdateTrade_ReturnsSuccessStatusCode()
    {
        var content = new StringContent("{\"id\":1,\"accountId\":1,\"securityCode\":\"APL\",\"timestamp\":\"2025-02-10T00:00:00Z\",\"amount\":100,\"buyOrSell\":\"Buy\",\"status\":1}", Encoding.UTF8, "application/json");
        var response = await _client.PutAsync("/api/trades/1", content);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteTrade_ReturnsSuccessStatusCode()
    {
        var response = await _client.DeleteAsync("/api/trades/1");
        response.EnsureSuccessStatusCode();
    }
}