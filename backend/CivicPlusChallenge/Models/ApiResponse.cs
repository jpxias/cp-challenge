namespace CivicPlusChallenge.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string? Error { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
    }
}
