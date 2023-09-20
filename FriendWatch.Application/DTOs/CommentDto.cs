namespace FriendWatch.Application.DTOs
{
    public record CommentDto
    {
        public int CommentId { get; set; }
        public int CommenterId { get; set; }
        public string CommenterName { get; set; } = string.Empty;
        public string? CommenterAvatarUrl { get; set; }
        public int WatchId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
