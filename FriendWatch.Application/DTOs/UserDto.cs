namespace FriendWatch.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? UserAvatarUrl { get; set; }
        public ImageFileDto? AvatarImageDto { get; set; }
    }
}
