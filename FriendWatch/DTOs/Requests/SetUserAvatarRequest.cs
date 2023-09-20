namespace FriendWatch.DTOs.Requests
{
    public record SetUserAvatarRequest
    {
        public IFormFile? UserAvatarImage { get; set; }
    }
}
