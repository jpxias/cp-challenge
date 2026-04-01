using FakeItEasy;
using Microsoft.Extensions.Options;
using CivicPlusChallenge.Configuration;
using System.Net;
using System.Net.Http.Json;

namespace CivicPlusChallenge.Tests
{
    [TestClass]
    public class ApiClientTests
    {
        private IHttpClientFactory _factory;
        private IOptions<EventsApiConfig> _options;
        private ApiClient _sut;

        public ApiClientTests()
        {
            _factory = A.Fake<IHttpClientFactory>();
            _options = Options.Create(new EventsApiConfig { Url = "https://api.test.com/" });
            _sut = new ApiClient(_factory, _options);
        }

        [TestMethod]
        public async Task GetAsync_WhenSuccessful_ShouldReturnData()
        {
            // Arrange
            var expectedData = new TestResponse { Name = "Test Event" };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedData)
            };

            var fakeHandler = new FakeHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(fakeHandler);

            A.CallTo(() => _factory.CreateClient(A<string>._)).Returns(httpClient);

            // Act
            var result = await _sut.GetAsync<TestResponse>("test-url");

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Test Event", result.Data?.Name);
        }

        [TestMethod]
        public async Task PostAsync_WhenBadRequest_ShouldReturnRawErrorString()
        {
            // Arrange
            string errorText = "Invalid Date Range";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(errorText)
            };

            var fakeHandler = new FakeHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(fakeHandler);
            A.CallTo(() => _factory.CreateClient(A<string>._)).Returns(httpClient);

            // Act
            var result = await _sut.PostAsync<object, TestResponse>("test-url", new { });

            // Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(errorText, result.Error);
            Assert.IsNull(result.Data);
        }
    }

    // Fake message hendler to mock HttpClient
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }

    public class TestResponse
    {
        public string Name { get; set; } = string.Empty;
    }
}