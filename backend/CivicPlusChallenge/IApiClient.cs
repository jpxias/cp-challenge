using CivicPlusChallenge.Models;

namespace CivicPlusChallenge
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, bool? useAuth = true);
        Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, bool? useAuth = true);
    }
}
