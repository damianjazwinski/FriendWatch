﻿namespace FriendWatch.Domain.Entities
{
    public class Circle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int OwnerId { get; set; }
        public int? ImageFileId { get; set; }
        public ImageFile? ImageFile { get; set; }
        public User Owner { get; set; }
        public List<User> Members { get; set; }
        public List<Invitation> SentInvitation { get; set; }
    }
}