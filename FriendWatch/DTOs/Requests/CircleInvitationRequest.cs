namespace FriendWatch.DTOs.Requests
{
    public record CircleInvitationRequest
    {
        public int CircleId { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
    }
}