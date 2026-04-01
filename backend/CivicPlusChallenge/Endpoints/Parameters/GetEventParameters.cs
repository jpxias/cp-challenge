using Microsoft.AspNetCore.Mvc;

namespace CivicPlusChallenge.Endpoints.Parameters
{
    public record GetEventParameters(
      [FromQuery(Name = "$top")] int? Top = 20,
      [FromQuery(Name = "$skip")] int? Skip = 0,
      [FromQuery(Name = "$filter")] string? Filter = null,
      [FromQuery(Name = "$orderBy")] string? OrderBy = null
  );
}
