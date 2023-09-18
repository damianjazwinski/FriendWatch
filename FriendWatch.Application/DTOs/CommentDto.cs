using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.DTOs
{
    public record CommentDto
    {
        public int CommentId { get; set; }
        public int CommenterId { get; set; }
        public string CommenterName { get; set; } = string.Empty;
        public int WatchId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
