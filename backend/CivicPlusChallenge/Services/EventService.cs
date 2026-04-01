using CivicPlusChallenge.Endpoints.Parameters;
using CivicPlusChallenge.Models;

namespace CivicPlusChallenge.Services
{
    public class EventService : IEventService
    {
        private readonly IApiClient _apiClient;

        public EventService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<GetEventResponse>> GetEvents(GetEventParameters query)
        {
            var queryParams = new Dictionary<string, string?>
            {
                { "$top", query.Top?.ToString() },
                { "$skip", query.Skip?.ToString() },
                { "$filter", query.Filter },
                { "$orderBy", query.OrderBy }
            };

            var urlWithQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("Events", queryParams);
            return await _apiClient.GetAsync<GetEventResponse>(urlWithQuery);
        }

        public async Task<ApiResponse<Event>> CreateEvent(SaveEventRequest newEvent)
        {
            return await _apiClient.PostAsync<SaveEventRequest, Event>("Events", newEvent);
        }
    }
}
