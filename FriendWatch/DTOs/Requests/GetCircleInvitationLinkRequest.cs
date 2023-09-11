namespace FriendWatch.DTOs.Requests
{
    public record GetCircleInvitationLinkRequest
    {
        public int CircleId { get; set; }
        public string? Message { get; set; }
    }
}