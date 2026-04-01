namespace CivicPlusChallenge.Models
{
    public class GetEventResponse
    {
        public int Total { get; set; }
        public IEnumerable<Event> items { get; set; } = Enumerable.Empty<Event>();
    }
}
