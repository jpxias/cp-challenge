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

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
