using System.Net.Http.Headers;

namespace CivicPlusChallenge.Services
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public AuthenticationHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetApiToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Access_token);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var newToken = await _tokenService.GetApiToken(forceRefresh: true);

                if (newToken != null)
                {
                    response.Dispose();
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken.Access_token);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }
    }
}
