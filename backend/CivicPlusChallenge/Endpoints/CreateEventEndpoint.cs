using CivicPlusChallenge.Models;
using CivicPlusChallenge.Services;

namespace CivicPlusChallenge.Endpoints
{
    public class CreateEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapPost("Events", GetEvents).Produces<ApiResponse<Event>>(StatusCodes.Status200OK);
        }

        private async Task<IResult> GetEvents(IEventService eventService, Event newEvent)
        {
            var createdEvent = await eventService.CreateEvent(new SaveEventRequest()
            {
                Title = newEvent.Title,
                Description = newEvent.Description,
                StartDate = newEvent.StartDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                EndDate = newEvent.EndDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            });
            return Results.Ok(createdEvent);
        }
    }
}