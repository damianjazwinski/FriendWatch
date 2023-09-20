namespace FriendWatch.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int CommenterId { get; set; }
        public User Commenter { get; set; }
        public int WatchId { get; set; }
        public Watch Watch { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
