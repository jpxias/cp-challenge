using Microsoft.Extensions.Caching.Memory;
using CivicPlusChallenge.Configuration;
using CivicPlusChallenge.Models;
using Microsoft.Extensions.Options;

namespace CivicPlusChallenge.Services
{
    public class TokenService : ITokenService
    {
        private readonly IApiClient _apiClient;
        private readonly EventsApiConfig _eventsApiConfig;
        private readonly IMemoryCache _cache;
        private const string TokenCacheKey = "CivicPlus_Api_Token";
        private const int TimeExpirationTHresholdSeconds = 30;

        public TokenService(IApiClient apiClient, IOptions<EventsApiConfig> eventsApiConfig, IMemoryCache cache)
        {
            _apiClient = apiClient;
            _eventsApiConfig = eventsApiConfig.Value;
            _cache = cache;
        }

        public async Task<AuthenticationResponse?> GetApiToken(bool forceRefresh = false)
        {
            if (_cache.TryGetValue(TokenCacheKey, out AuthenticationResponse? cachedToken) && !forceRefresh)
            {
                return cachedToken;
            }

            var response = await _apiClient.PostAsync<EventsApiConfig, AuthenticationResponse>("Auth", new EventsApiConfig()
            {
                ClientID = _eventsApiConfig.ClientID,
                ClientSecret = _eventsApiConfig.ClientSecret
            }, false);

            var token = response.Data;

            if (token != null && !string.IsNullOrEmpty(token.Access_token))
            {
                // Set buffer: Expire the cache 30 seconds before the actual token expires 
                // to avoid race conditions with the server.
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(token.Expires_in - TimeExpirationTHresholdSeconds));

                _cache.Set(TokenCacheKey, token, cacheOptions);
            }

            return token;
        }
    }
}