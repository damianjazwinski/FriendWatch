namespace FriendWatch.DTOs.Requests
{
    public record CircleInvitationRequest
    {
        public int CircleId { get; set; }
        public string ReceiverUsername { get; set; } = string.Empty;
    }
}