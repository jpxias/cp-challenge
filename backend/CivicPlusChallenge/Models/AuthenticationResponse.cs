namespace CivicPlusChallenge.Models
{
    public class AuthenticationResponse
    {
        public string Access_token { get; set; } = string.Empty;
        public int Expires_in { get; set; }
    }
}
