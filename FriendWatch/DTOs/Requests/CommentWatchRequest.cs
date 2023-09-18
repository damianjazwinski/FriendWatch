namespace FriendWatch.DTOs.Requests
{
    public record CommentWatchRequest
    {
        public int WatchId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
