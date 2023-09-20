namespace FriendWatch.DTOs.Responses
{
    public record GetUserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? UserAvatarUrl { get; set; }
    }
}
