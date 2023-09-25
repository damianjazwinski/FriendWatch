using FriendWatch.Application.DTOs;

namespace FriendWatch.DTOs.Responses
{
    public class WatchResponseDto
    {
        public int WatchId { get; set; }
        public int CircleId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ExternalLink { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CircleName { get; set; } = string.Empty;
        public List<CommentDto> Comments { get; set; } = new();
        public string? CreatorAvatarUrl { get; set; }
    }
}
