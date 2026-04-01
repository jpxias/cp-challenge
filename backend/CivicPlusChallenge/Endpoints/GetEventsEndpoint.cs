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
            builder.MapGet("Events", GetEvents).Produces<ApiResponse<GetEventResponse>>(StatusCodes.Status200OK);
        }

        private async Task<IResult> GetEvents([AsParameters] GetEventParameters query, IEventService eventService)
        {
            var events = await eventService.GetEvents(query);
            return Results.Ok(events);
        }
    }
}