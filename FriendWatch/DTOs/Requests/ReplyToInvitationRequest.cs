namespace FriendWatch.DTOs.Requests
{
    public record ReplyToInvitationRequest
    {
        public int InvitationId { get; set; }
        public bool Acceptance { get; set; }
    }
}
