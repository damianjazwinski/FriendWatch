namespace FriendWatch.DTOs.Responses
{
    public record LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
