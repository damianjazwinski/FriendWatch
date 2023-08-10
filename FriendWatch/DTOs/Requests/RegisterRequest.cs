namespace FriendWatch.DTOs.Requests
{
    public record RegisterRequest
    {
        public string Username { get; set; }
        public string ConfirmUsername { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
