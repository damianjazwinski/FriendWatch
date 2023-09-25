namespace FriendWatch.Application.DTOs
{
    public record WatchDto
    {
        public int? Id { get; set; }
        public int CircleId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ExternalLink { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int CreatorId { get; set; }
        public UserDto? Creator { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CircleDto? Circle { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
    }
}
