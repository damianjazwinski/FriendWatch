namespace FriendWatch.DTOs.Responses
{
    public record AuthResponse
    {
        public long AccessTokenExpiration { get; set; }
        public long RefreshTokenExpiration { get; set; }
    }
}
