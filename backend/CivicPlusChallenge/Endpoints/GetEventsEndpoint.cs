using CivicPlusChallenge.Endpoints.Parameters;
using CivicPlusChallenge.Models;
using CivicPlusChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivicPlusChallenge.Endpoints
{
    public class GetEventsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapGet("Events", GetEvents)
                .Produces<ApiResponse<GetEventResponse>>(StatusCodes.Status200OK)
                .Produces<ApiResponse<GetEventResponse>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> GetEvents([AsParameters] GetEventParameters query, IEventService eventService)
        {
            try
            {
                var response = await eventService.GetEvents(query);

                if (!response.IsSuccess)
                {
                    return Results.Json(response, statusCode: response.StatusCode > 0 ? response.StatusCode : StatusCodes.Status500InternalServerError);
                }

                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(new ApiResponse<GetEventResponse>() { Error = e.Message });
            }
        }
    }
}