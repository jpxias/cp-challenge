using CivicPlusChallenge.Endpoints.Parameters;
using CivicPlusChallenge.Models;

namespace CivicPlusChallenge.Services
{
    public interface IEventService
    {
        Task<ApiResponse<GetEventResponse>> GetEvents(GetEventParameters query);
        Task<ApiResponse<Event>> CreateEvent(Event newEvent);
    }
}
