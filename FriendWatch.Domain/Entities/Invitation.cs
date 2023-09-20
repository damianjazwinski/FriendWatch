namespace FriendWatch.Domain.Entities
{
    public class Invitation
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public bool? IsAccepted { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public int CircleId { get; set; }
        public Circle Circle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
