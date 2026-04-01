using CivicPlusChallenge.Configuration;
using CivicPlusChallenge.Models;
using CivicPlusChallenge.Services;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CivicPlusChallenge.Tests.Services
{
    [TestClass]
    public class TokenServiceTests
    {
        private IApiClient _apiClient;
        private IOptions<EventsApiConfig> _options;
        private IMemoryCache _cache;
        private TokenService _sut;
        public TokenServiceTests()
        {
            _apiClient = A.Fake<IApiClient>();
            _cache = A.Fake<IMemoryCache>();
            var config = new EventsApiConfig
            {
                ClientID = "test-client-id",
                ClientSecret = "test-secret"
            };
            _options = Options.Create(config);

            _sut = new TokenService(_apiClient, _options, _cache);
        }

        [TestMethod]
        public async Task GetApiToken_ShouldCallAuthEndpoint_WithCorrectCredentials()
        {
            // Arrange
            var expectedResponse = new ApiResponse<AuthenticationResponse>
            {
                StatusCode = 200,
                Data = new AuthenticationResponse { Access_token = "fake-jwt-token", Expires_in = 5000 }
            };

            A.CallTo(() => _apiClient.PostAsync<EventsApiConfig, AuthenticationResponse>(
                "Auth",
                A<EventsApiConfig>.That.Matches(c => c.ClientID == "test-client-id" && c.ClientSecret == "test-secret"),
                false))
                .Returns(expectedResponse);

            // Act
            var result = await _sut.GetApiToken();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("fake-jwt-token", result.Access_token);

            // Verify the call happened exactly once
            A.CallTo(() => _apiClient.PostAsync<EventsApiConfig, AuthenticationResponse>("Auth", A<EventsApiConfig>._, false))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task GetApiToken_WhenApiFails_ShouldReturnNull()
        {
            // Arrange
            var errorResponse = new ApiResponse<AuthenticationResponse>
            {
                StatusCode = 401,
                Data = null
            };

            A.CallTo(() => _apiClient.PostAsync<EventsApiConfig, AuthenticationResponse>(A<string>._, A<EventsApiConfig>._, A<bool?>._))
                .Returns(errorResponse);

            // Act
            var result = await _sut.GetApiToken();

            // Assert
            Assert.IsNull(result);
        }
    }
}