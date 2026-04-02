using CivicPlusChallenge.Models;
using CivicPlusChallenge.Services;

namespace CivicPlusChallenge.Endpoints
{
    public class CreateEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapPost("Events", GetEvents)
                .Produces<ApiResponse<Event>>(StatusCodes.Status200OK)
                .Produces<ApiResponse<Event>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> GetEvents(IEventService eventService, Event newEvent)
        {
            try
            {
                var response = await eventService.CreateEvent(new SaveEventRequest()
                {
                    Title = newEvent.Title,
                    Description = newEvent.Description,
                    StartDate = newEvent.StartDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    EndDate = newEvent.EndDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                });

                if (!response.IsSuccess)
                {
                    return Results.Json(response, statusCode: response.StatusCode > 0 ? response.StatusCode : StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception e)
            {
                return Results.InternalServerError(new ApiResponse<Event>() { Error = e.Message });
            }

            return Results.Ok();
        }
    }
}