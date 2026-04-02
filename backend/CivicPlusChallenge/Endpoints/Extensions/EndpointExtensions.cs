using System.Reflection;

namespace CivicPlusChallenge.Endpoints.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var endpointTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t));

            foreach (var type in endpointTypes)
            {
                var endpoint = (IEndpoint)Activator.CreateInstance(type)!;
                endpoint.MapEndpoint(app);
            }
        }
    }
}
