using CivicPlusChallenge.Endpoints.Extensions;
using CivicPlusChallenge.Models;
using CivicPlusChallenge.Services;

namespace CivicPlusChallenge.Endpoints
{
    public class CreateEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapPost("Events", CreateEvent)
                .Produces<ApiResponse<Event>>(StatusCodes.Status200OK)
                .Produces<ApiResponse<Event>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> CreateEvent(IEventService eventService, Event newEvent)
        {
            try
            {
                var response = await eventService.CreateEvent(newEvent);

                if (!response.IsSuccess)
                {
                    return Results.Json(response, statusCode: response.StatusCode > 0 ? response.StatusCode : StatusCodes.Status500InternalServerError);
                }

                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(new ApiResponse<Event>() { Error = e.Message });
            }
        }
    }
}