namespace FriendWatch.DTOs.Responses
{
    public record LoginResponse
    {
        public long AccessTokenExpiration { get; set; }
        public long RefreshTokenExpiration { get; set; }
    }
}
