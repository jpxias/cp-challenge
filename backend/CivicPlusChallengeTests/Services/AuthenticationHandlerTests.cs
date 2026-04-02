using FakeItEasy;
using CivicPlusChallenge.Services;
using CivicPlusChallenge.Models;
using System.Net;

namespace CivicPlusChallenge.Tests.Services
{
    [TestClass]
    public class AuthenticationHandlerTests
    {
        private ITokenService _tokenService;
        private AuthenticationHandler _sut;

        public AuthenticationHandlerTests()
        {
            _tokenService = A.Fake<ITokenService>();
            _sut = new AuthenticationHandler(_tokenService);
        }

        [TestMethod]
        public async Task SendAsync_ShouldAddBearerToken_WhenTokenIsReturnedByService()
        {
            // Arrange
            var expectedToken = "test-bearer-token-123";
            var tokenResponse = new AuthenticationResponse { Access_token = expectedToken };

            A.CallTo(() => _tokenService.GetApiToken())
                .Returns(Task.FromResult<AuthenticationResponse?>(tokenResponse)!);

            // We use a DelegatingHandler's inner handler to capture the request
            var innerHandler = new TestHandler();
            _sut.InnerHandler = innerHandler;

            var client = new HttpClient(_sut);

            // Act
            await client.GetAsync("http://localhost/api/test");

            // Assert
            var request = innerHandler.LastRequest;
            Assert.IsNotNull(request.Headers.Authorization);
            Assert.AreEqual("Bearer", request.Headers.Authorization.Scheme);
            Assert.AreEqual(expectedToken, request.Headers.Authorization.Parameter);

            A.CallTo(() => _tokenService.GetApiToken()).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task SendAsync_ShouldProceedEvenIfTokenIsMissing()
        {
            // Arrange
            A.CallTo(() => _tokenService.GetApiToken())
                .Returns(Task.FromResult<AuthenticationResponse?>(null)!);

            var innerHandler = new TestHandler();
            _sut.InnerHandler = innerHandler;
            var client = new HttpClient(_sut);

            // Act
            await client.GetAsync("http://localhost/api/test");

            // Assert
            var request = innerHandler.LastRequest;
            Assert.IsNull(request.Headers.Authorization?.Parameter);
        }

        [TestMethod]
        public async Task SendAsync_WhenUnauthorized_ShouldForceRefreshAndRetry()
        {
            // Arrange
            var oldToken = new AuthenticationResponse { Access_token = "expired_token" };
            var newToken = new AuthenticationResponse { Access_token = "fresh_token" };

            var tokenService = A.Fake<ITokenService>();
            A.CallTo(() => tokenService.GetApiToken()).Returns(oldToken);
            A.CallTo(() => tokenService.GetApiToken(true)).Returns(newToken);

            var innerHandler = A.Fake<HttpMessageHandler>();

            // Setup sequence for the HTTP responses
            // 1st call: 400 Bad Request
            // 2nd call: 200 OK
            A.CallTo(innerHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .ReturnsNextFromSequence(
                    Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)),
                    Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))
                );

            var handler = new AuthenticationHandler(tokenService)
            {
                InnerHandler = innerHandler
            };

            var invoker = new HttpMessageInvoker(handler);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/events");

            // Act
            var result = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            A.CallTo(() => tokenService.GetApiToken(true)).MustHaveHappenedOnceExactly();
            A.CallTo(innerHandler)
                .Where(x => x.Method.Name == "SendAsync")
                .MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual("fresh_token", request.Headers.Authorization?.Parameter);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        private class TestHandler : DelegatingHandler
        {
            public HttpRequestMessage LastRequest { get; private set; } = null!;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        }
    }
}