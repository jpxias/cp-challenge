using CivicPlusChallenge.Configuration;
using CivicPlusChallenge.Models;
using Microsoft.Extensions.Options;

namespace CivicPlusChallenge
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EventsApiConfig _eventsApiConfig;

        public ApiClient(IHttpClientFactory httpClientFactory, IOptions<EventsApiConfig> eventsApiConfig)
        {
            _httpClientFactory = httpClientFactory;
            _eventsApiConfig = eventsApiConfig.Value;
        }

        private HttpClient GetHttpClient(bool? useAuth)
        {
            HttpClient httpClient;

            if (useAuth.HasValue && useAuth.Value)
            {
                httpClient = _httpClientFactory.CreateClient("AuthenticatedHttpClient");
            }
            else
            {
                httpClient = _httpClientFactory.CreateClient();
            }

            if (_eventsApiConfig.Url == null) throw new InvalidOperationException("Invalid API Url");

            if (httpClient.BaseAddress == null) httpClient.BaseAddress = new Uri(_eventsApiConfig.Url);

            return httpClient;
        }

        public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, bool? useAuth = true)
        {
            var httpClient = GetHttpClient(useAuth);
            var response = await httpClient.GetAsync(url);

            var result = new ApiResponse<TResponse>
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                result.Data = await response.Content.ReadFromJsonAsync<TResponse>();
            }
            else
            {
                result.Error = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, bool? useAuth = true)
        {
            var httpClient = GetHttpClient(useAuth);
            var response = await httpClient.PostAsJsonAsync(url, request);

            var result = new ApiResponse<TResponse>
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                result.Data = await response.Content.ReadFromJsonAsync<TResponse>();
            }
            else
            {
                result.Error = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
    }
}
