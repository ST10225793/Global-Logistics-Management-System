using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Global_Logistics_Management_System.Models;
using System.Collections.Generic;

namespace Global_Logistics_Management_System.Tests
{
    // WebApplicationFactory boots your real Web API project up in memory automatically
    public class ContractsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContractsApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Creates an internal network client to query the in-memory API
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContracts_ReturnsSuccessStatusCode_AndNonNullJson()
        {
            // Act: Fire a real GET request at the running API endpoint
            var response = await _client.GetAsync("/api/ContractsApi");

            // 🔍 DIAGNOSTIC: If it fails, read the actual backend exception stack trace
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"API Crashed with 500 Internal Server Error. Details:\n{errorContent}");
            }

            // Assert 1: Verify the HTTP Status Code is 200 OK
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert 2: Verify the returned JSON is not null and parses cleanly
            var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>();
            Assert.NotNull(contracts);
        }
    }
}