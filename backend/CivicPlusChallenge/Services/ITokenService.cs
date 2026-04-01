using CivicPlusChallenge.Models;

namespace CivicPlusChallenge.Services
{
    public interface ITokenService
    {
        Task<AuthenticationResponse?> GetApiToken();
    }
}
