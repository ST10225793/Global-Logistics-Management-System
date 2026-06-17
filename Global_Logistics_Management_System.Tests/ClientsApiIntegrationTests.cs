using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System.Tests
{
    public class ClientsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ClientsApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Instantiates an unmocked, isolated test client rooted to the target Web API assembly
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateThenReadIntegrationLoop_VerifiesDataIntegrity()
        {
            // --- 1. ARRANGE: Structure clean entity model values
            var mockClient = new Client
            {
                Name = "Automated Integration Test Unit",
                Email = "integration.test@globallogistics.com"
            };

            // --- 2. ACT (CREATE): Direct POST execution to the REST routing path
            var postResponse = await _client.PostAsJsonAsync("api/ClientsApi", mockClient);

            // ASSERT: Validate professional API structure via accurate HTTP Status Codes
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode); // Confirms HTTP 201 Created

            var createdClient = await postResponse.Content.ReadFromJsonAsync<Client>();
            Assert.NotNull(createdClient);
            Assert.True(createdClient.ClientId > 0); // Proves the database successfully committed and indexed the row

            // --- 3. ACT (READ): Query the database using the freshly generated identifier index
            var getResponse = await _client.GetAsync($"api/ClientsApi/{createdClient.ClientId}");

            // ASSERT: Verify successful retrieval status and absolute data validation
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode); // Confirms HTTP 200 OK

            var retrievedClient = await getResponse.Content.ReadFromJsonAsync<Client>();
            Assert.NotNull(retrievedClient);

            // Core Integrity Checks (Ensures stored properties exactly mirror input vectors)
            Assert.Equal(mockClient.Name, retrievedClient.Name);
            Assert.Equal(mockClient.Email, retrievedClient.Email);
        }
    }
}