using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TradeManagementApp.Tests.Integration
{
    public class ApiTests : IClassFixture<CustomWebApplicationFactory<TradeManagementApp.API.Startup>>
    {
        private readonly CustomWebApplicationFactory<TradeManagementApp.API.Startup> _factory;

        public ApiTests(CustomWebApplicationFactory<TradeManagementApp.API.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetTradeById_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            int tradeId = 1; // Assuming there is a trade with ID 1 in the test database

            // Act
            var response = await client.GetAsync($"/api/trades/{tradeId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAccountById_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            int accountId = 1; // Assuming there is an account with ID 1 in the test database

            // Act
            var response = await client.GetAsync($"/api/accounts/{accountId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip ="True")]
        public async Task CreateAccount_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = new StringContent(@"{""firstName"":""Test"",""lastName"":""User""}", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/accounts", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAccount_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            int accountId = 1; // Assuming there is an account with ID 1 in the test database

            // Act
            var response = await client.DeleteAsync($"/api/accounts/{accountId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(Skip ="True")]
        public async Task UpdateAccount_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            int accountId = 1; // Assuming there is an account with ID 1 in the test database
            var content = new StringContent(@"{""id"":1,""firstName"":""Updated"",""lastName"":""User""}", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/api/accounts/{accountId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(Skip ="True")]
        public async Task CreateTrade_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = new StringContent(@"{""accountId"":1,""securityCode"":""APL"",""timestamp"":""2024-01-01T00:00:00"",""amount"":100,""buyOrSell"":""Buy"",""status"":""Open""}", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/trades", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}