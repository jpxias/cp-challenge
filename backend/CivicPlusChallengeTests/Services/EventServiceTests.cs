using FakeItEasy;
using CivicPlusChallenge.Services;
using CivicPlusChallenge.Models;
using CivicPlusChallenge.Endpoints.Parameters;

namespace CivicPlusChallenge.Tests.Services
{
    [TestClass]
    public class EventServiceTests
    {
        private IApiClient _apiClient;
        private EventService _sut;

        public EventServiceTests()
        {
            _apiClient = A.Fake<IApiClient>();
            _sut = new EventService(_apiClient);
        }

        [TestMethod]
        public async Task GetEvents_WithParameters_ShouldConstructCorrectQueryString()
        {
            // Arrange
            var query = new GetEventParameters
            {
                Top = 10,
                Skip = 20,
                Filter = "Name eq 'Test'"
            };

            var expectedResponse = new ApiResponse<GetEventResponse> { StatusCode = 200 };

            A.CallTo(() => _apiClient.GetAsync<GetEventResponse>(
                A<string>.That.Contains("$top=10"),
                true))
                .Returns(expectedResponse);

            // Act
            var result = await _sut.GetEvents(query);

            // Assert
            Assert.AreEqual(200, result.StatusCode);

            // Verify the call happened exactly once
            A.CallTo(() => _apiClient.GetAsync<GetEventResponse>(
                A<string>.That.Contains("$skip=20"),
                true))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task CreateEvent_ShouldForwardRequestToClient()
        {
            // Arrange
            var newEvent = new Event { Title = "Event Test" };
            var apiResponse = new ApiResponse<Event> { StatusCode = 201 };

            A.CallTo(() => _apiClient.PostAsync<Event, Event>("Events", newEvent, true))
                .Returns(apiResponse);

            // Act
            var result = await _sut.CreateEvent(newEvent);

            // Assert
            Assert.AreEqual(201, result.StatusCode);
            A.CallTo(() => _apiClient.PostAsync<Event, Event>("Events", newEvent, true))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task GetEvents_WhenClientFails_ShouldReturnError()
        {
            // Arrange
            var query = new GetEventParameters();
            var errorResponse = new ApiResponse<GetEventResponse>
            {
                StatusCode = 400,
                Error = "Error"
            };

            A.CallTo(() => _apiClient.GetAsync<GetEventResponse>(A<string>._, true))
                .Returns(errorResponse);

            // Act
            var result = await _sut.GetEvents(query);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Error", result.Error);
        }
    }
}