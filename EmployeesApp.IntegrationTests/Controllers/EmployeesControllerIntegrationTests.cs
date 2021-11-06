using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EmployeesApp.IntegrationTests.Controllers
{
    public class EmployeesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public EmployeesControllerIntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsAnApplicationForm()
        {
            var response = await _client.GetAsync("/Employees");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Mark", content);

            Assert.Contains("Evelin", content);
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsCreateForm()
        {
            var response = await _client.GetAsync("/Employees/Create");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Please provide a new employee data", content);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewWithErrorMessage()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create");

            var formModel = new Dictionary<string, string>
            {
                { "Name", "New Employee" },

                { "Age", "25" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Account number is required", responseString);
        }

        [Fact]
        public async Task Create_WhenPOSTExecuted_ReturnsToIndexViewWithCreatedEmployee()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create");

            var formModel = new Dictionary<string, string>
            {
                {"Name", "Alex King" },
                {"Age", "23" },
                { "AccountNumber", "214-5874986532-21" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadAsStringAsync();

            Assert.Contains("Alex King", result);

            Assert.Contains("214-5874986532-21", result);
        }
    }
}
