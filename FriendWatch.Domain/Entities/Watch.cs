using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendWatch.Domain.Entities
{
    public class Watch
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int CircleId { get; set; }
        public Circle Circle { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ExternalLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
    }
}
