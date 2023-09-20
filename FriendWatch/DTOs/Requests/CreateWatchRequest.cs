namespace FriendWatch.DTOs.Requests
{
    public record CreateWatchRequest
    {
        public int CircleId { get; set; }
        public string? ExternalLink { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
